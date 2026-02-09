import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { getToken } from '../../auth/auth'
import { useAuth } from '../../auth/AuthContext'
import './ComponentList.css'
import '../../pages/Configs.css'

const API_BASE_URL = 'http://localhost:5271/api'

function pick(v) {
  return v || v === 0 ? v : ''
}

// Define field schemas for each component type
const fieldSchemas = {
  'CPU': [
    { name: 'Model', type: 'text', required: true },
    { name: 'Power', type: 'number', required: false },
    { name: 'Cores', type: 'number', required: false },
    { name: 'Price', type: 'number', required: false, step: '0.01' }
  ],
  'GPU': [
    { name: 'Brand', type: 'text', required: true },
    { name: 'Model', type: 'text', required: true },
    { name: 'Power', type: 'number', required: false },
    { name: 'Volts', type: 'number', required: false },
    { name: 'Price', type: 'number', required: false, step: '0.01' }
  ],
  'Mobo': [
    { name: 'Model', type: 'text', required: true },
    { name: 'Chip', type: 'text', required: true },
    { name: 'Rating', type: 'text', required: false },
    { name: 'Price', type: 'number', required: false, step: '0.01' }
  ],
  'Pcu': [
    { name: 'Model', type: 'text', required: true },
    { name: 'Volts', type: 'number', required: false },
    { name: 'Rating', type: 'text', required: false },
    { name: 'Price', type: 'number', required: false, step: '0.01' }
  ],
  'Ram': [
    { name: 'Model', type: 'text', required: true },
    { name: 'Gigabytes', type: 'number', required: false },
    { name: 'Speed', type: 'number', required: false },
    { name: 'Price', type: 'number', required: false, step: '0.01' }
  ],
  'SSD': [
    { name: 'Brand', type: 'text', required: true },
    { name: 'Model', type: 'text', required: true },
    { name: 'SPD', type: 'number', required: false },
    { name: 'Capacity', type: 'number', required: false },
    { name: 'Price', type: 'number', required: false, step: '0.01' }
  ]
}

export default function ComponentList({ resource, displayName }) {
  const navigate = useNavigate()
  const { isAdmin, refreshAuth } = useAuth()
  const [items, setItems] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)
  const [showCreateModal, setShowCreateModal] = useState(false)
  const [showEditModal, setShowEditModal] = useState(false)
  const [editingItem, setEditingItem] = useState(null)
  const [creating, setCreating] = useState(false)
  const [editing, setEditing] = useState(false)
  const [formData, setFormData] = useState({})
  const [editPrice, setEditPrice] = useState('')

  // Initialize form data based on resource type
  useEffect(() => {
    const schema = fieldSchemas[resource] || []
    const initialData = {}
    schema.forEach(field => {
      initialData[field.name] = ''
    })
    setFormData(initialData)
  }, [resource])

  const fetchItems = () => {
    const token = getToken()
    const controller = new AbortController()
    setLoading(true)
    setError(null)

    fetch(`${API_BASE_URL}/${resource}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
      },
      signal: controller.signal,
    })
      .then(async (res) => {
        if (!res.ok) {
          const txt = await res.text().catch(() => '')
          throw new Error(`HTTP ${res.status} ${txt}`)
        }
        return res.json()
      })
      .then((data) => {
        setItems(Array.isArray(data) ? data : [])
      })
      .catch((err) => {
        if (err.name !== 'AbortError') {
          console.error('Error fetching', resource, err)
          setError(err.message || 'Failed to load')
        }
      })
      .finally(() => setLoading(false))

    return () => controller.abort()
  }

  useEffect(() => {
    const abortFn = fetchItems()
    return () => abortFn && abortFn.abort && abortFn.abort()
  }, [resource])

  const handleDelete = async (id) => {
    if (!window.confirm('Delete this item?')) return
    const token = getToken()
    try {
      const res = await fetch(`${API_BASE_URL}/${resource}/${id}`, {
        method: 'DELETE',
        headers: {
          ...(token ? { Authorization: `Bearer ${token}` } : {}),
        },
      })
      if (!res.ok) {
        const txt = await res.text().catch(() => '')
        throw new Error(`HTTP ${res.status} ${txt}`)
      }
      setItems((prev) => prev.filter((it) => (it.id ?? it.Id ?? it.ID) !== id))
    } catch (err) {
      console.error('Delete failed', err)
      setError(err.message || 'Delete failed')
    }
  }

  const handleUpdate = (it) => {
    const id = it.id ?? it.Id ?? it.ID
    const currentPrice = it.price ?? it.Price ?? 0
    setEditingItem(it)
    setEditPrice(currentPrice.toString())
    setShowEditModal(true)
  }

  const handleEditSubmit = async (e) => {
    e.preventDefault()
    if (!editingItem) return

    setEditing(true)
    const token = getToken()
    const id = editingItem.id ?? editingItem.Id ?? editingItem.ID
    
    try {
      const res = await fetch(`${API_BASE_URL}/${resource}/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          ...(token ? { Authorization: `Bearer ${token}` } : {}),
        },
        body: JSON.stringify({ price: parseFloat(editPrice) })
      })
      if (!res.ok) {
        const txt = await res.text().catch(() => '')
        throw new Error(`HTTP ${res.status} ${txt}`)
      }
      const updated = await res.json().catch(() => null)
      setItems((prev) => prev.map((p) => ((p.id ?? p.Id ?? p.ID) === id ? (updated || { ...p, Price: editPrice }) : p)))
      setShowEditModal(false)
      setEditingItem(null)
      setEditPrice('')
    } catch (err) {
      console.error('Update failed', err)
      setError(err.message || 'Update failed')
    } finally {
      setEditing(false)
    }
  }

  const handleCreateSubmit = async (e) => {
    e.preventDefault()
    
    // Validate required fields
    const schema = fieldSchemas[resource] || []
    for (const field of schema) {
      if (field.required && !formData[field.name]) {
        setError(`${field.name} is required`)
        return
      }
    }

    setCreating(true)
    const token = getToken()
    
    try {
      // Build payload with correct types
      const payload = {}
      schema.forEach(field => {
        if (formData[field.name] !== '') {
          if (field.type === 'number') {
            payload[field.name] = parseFloat(formData[field.name])
          } else {
            payload[field.name] = formData[field.name]
          }
        }
      })

      const res = await fetch(`${API_BASE_URL}/${resource}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          ...(token ? { Authorization: `Bearer ${token}` } : {}),
        },
        body: JSON.stringify(payload),
      })
      
      if (!res.ok) {
        const txt = await res.text().catch(() => '')
        throw new Error(`HTTP ${res.status} ${txt}`)
      }
      
      const created = await res.json().catch(() => null)
      if (created) {
        setItems((prev) => [created, ...prev])
        setShowCreateModal(false)
        // Reset form
        const initialData = {}
        schema.forEach(field => {
          initialData[field.name] = ''
        })
        setFormData(initialData)
      } else {
        fetchItems()
      }
    } catch (err) {
      console.error('Create failed', err)
      setError(err.message || 'Create failed')
    } finally {
      setCreating(false)
    }
  }

  const handleMore = (id) => {
    // Map resource to route
    const resourceToRoute = {
      'CPU': 'cpu',
      'GPU': 'gpu',
      'Mobo': 'motherboards',
      'Ram': 'ram',
      'SSD': 'ssd',
      'Pcu': 'pcu'
    }

    const route = resourceToRoute[resource]
    navigate(`/components/${route}/${id}`)
  }

  return (
    <div className="configs-page">
      <div className="configs-header">
        <h1>{displayName || resource}</h1>
        <div className="header-right">
          {isAdmin && <button className="btn-new-config" onClick={() => setShowCreateModal(true)}>Create</button>}
        </div>
      </div>

      <div className="configs-main">
        {loading && <div className="configs-message">Loading {displayName || resource}…</div>}
        {error && <div className="configs-message error">{error}</div>}
        {!loading && !error && items.length === 0 && (
          <div className="configs-message">No items found</div>
        )}

        <div className={`configs-grid vertical-list`}>
          {items.map((it) => {
            // common DTO properties: Model, Brand, Price, Chip, Id, Photo
            const id = it.id ?? it.Id ?? it.ID ?? it.id
            const model = pick(it.model) || pick(it.Model) || pick(it.name) || pick(it.Name)
            const brand = pick(it.brand) || pick(it.Brand) || ''
            const chip = pick(it.chip) || pick(it.Chip) || ''
            const price = pick(it.price) || pick(it.Price) || ''
            const photo = pick(it.photo) || pick(it.Photo) || ''
            const photoUrl = photo ? `/Photos/${photo}` : null

            return (
              <div className="config-card horizontal-card" key={id}>
                <div className="card-content">
                  <div className="card-header-row">
                    <h3 className="config-name">{model || `${displayName} ${id}`}</h3>
                    <div style={{ marginLeft: 'auto', display: 'flex', gap: '8px' }}>
                      <button className="btn-more" onClick={() => handleMore(id)}>More</button>
                      {isAdmin && (
                        <>
                          <button className="btn-delete" onClick={() => handleDelete(id)}>Delete</button>
                          <button className="btn-more" onClick={() => handleUpdate(it)}>Update</button>
                        </>
                      )}
                    </div>
                  </div>
                  <div className="card-details-inline">
                    <div className="detail-row">
                      <div className="value">{brand}</div>
                    </div>
                    {chip && (
                      <div className="detail-row">
                        <div className="value">{chip}</div>
                      </div>
                    )}
                  </div>
                  <div className="price-accent">Price: {price}</div>
                </div>
              </div>
            )
          })}
        </div>
      </div>

      {/* Create Modal */}
      {showCreateModal && (
        <div className="modal-overlay" onClick={() => setShowCreateModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h2>Create New {displayName}</h2>
              <button className="modal-close" onClick={() => setShowCreateModal(false)}>×</button>
            </div>

            <form onSubmit={handleCreateSubmit} className="modal-form">
              {(fieldSchemas[resource] || []).map(field => (
                <div key={field.name} className="form-group">
                  <label>
                    {field.name}
                    {field.required && <span style={{ color: '#ff4444' }}>*</span>}
                  </label>
                  <input
                    type={field.type}
                    name={field.name}
                    value={formData[field.name] || ''}
                    onChange={(e) => setFormData({
                      ...formData,
                      [field.name]: e.target.value
                    })}
                    step={field.step}
                    required={field.required}
                    placeholder={`Enter ${field.name.toLowerCase()}`}
                  />
                </div>
              ))}

              <div className="modal-actions">
                <button type="button" className="btn-cancel" onClick={() => setShowCreateModal(false)}>
                  Cancel
                </button>
                <button type="submit" className="btn-submit" disabled={creating}>
                  {creating ? 'Creating...' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Edit Modal */}
      {showEditModal && (
        <div className="modal-overlay" onClick={() => setShowEditModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h2>Edit Price</h2>
              <button className="modal-close" onClick={() => setShowEditModal(false)}>×</button>
            </div>

            <form onSubmit={handleEditSubmit} className="modal-form">
              <div className="form-group">
                <label>Price</label>
                <input
                  type="number"
                  value={editPrice}
                  onChange={(e) => setEditPrice(e.target.value)}
                  step="0.01"
                  placeholder="Enter price"
                />
              </div>

              <div className="modal-actions">
                <button type="button" className="btn-cancel" onClick={() => setShowEditModal(false)}>
                  Cancel
                </button>
                <button type="submit" className="btn-submit" disabled={editing}>
                  {editing ? 'Updating...' : 'Update'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}
