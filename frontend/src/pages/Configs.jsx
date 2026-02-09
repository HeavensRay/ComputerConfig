import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import './Configs.css'

const API_BASE_URL = 'http://localhost:5271/api'

// Component cache to store fetched component data
const componentCache = {}

export default function Configs() {
  const navigate = useNavigate()
  const username = localStorage.getItem('username')
  const token = localStorage.getItem('token')

  const [configs, setConfigs] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [componentsLoaded, setComponentsLoaded] = useState(false)
  
  // Modal states
  const [notification, setNotification] = useState(null)
  const [showCreateModal, setShowCreateModal] = useState(false)
  const [creating, setCreating] = useState(false)
  
  const [newConfigForm, setNewConfigForm] = useState({
    configName: '',
    purpose: '1080',
    price: '',
    ssdSize: '500'
  })

  const [availableComponents, setAvailableComponents] = useState({
    cpu: [],
    gpu: [],
    mobo: [],
    ram: [],
    ssd: [],
    pcu: []
  })

  useEffect(() => {
    fetchConfigs()
    fetchAvailableComponents()
  }, [])

  const showNotification = (message, type = 'success') => {
    setNotification({ message, type })
    setTimeout(() => setNotification(null), 3000)
  }

  const fetchAvailableComponents = async () => {
    const componentTypes = ['CPU', 'GPU', 'Mobo', 'Ram', 'SSD', 'Pcu']
    const resourceMap = {
      'CPU': 'cpu',
      'GPU': 'gpu',
      'Mobo': 'Mobo',
      'Ram': 'ram',
      'SSD': 'ssd',
      'Pcu': 'pcu'
    }

    try {
      for (const type of componentTypes) {
        const resource = resourceMap[type]
        const response = await fetch(`${API_BASE_URL}/${resource}`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()
          const key = type.toLowerCase()
          setAvailableComponents(prev => ({
            ...prev,
            [key === 'mobo' ? 'mobo' : key]: Array.isArray(data) ? data : []
          }))
        }
      }
    } catch (err) {
      console.error('Error fetching components:', err)
    }
  }

  const fetchConfigs = async () => {
    setLoading(true)
    setError(null)

    try {
      const response = await fetch(`${API_BASE_URL}/config`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      })

      if (!response.ok) {
        throw new Error(`Failed to fetch configs: ${response.status}`)
      }

      const data = await response.json()
      setConfigs(Array.isArray(data) ? data : [])

      // Fetch all components for the configs
      if (Array.isArray(data)) {
        await fetchAllComponents(data)
        setComponentsLoaded(true)
      }
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  const fetchAllComponents = async (configs) => {
    const allIds = {
      cpu: new Set(),
      gpu: new Set(),
      mobo: new Set(),
      ram: new Set(),
      ssd: new Set(),
      pcu: new Set()
    }

    // Collect all unique component IDs
    configs.forEach((config) => {
      if (config.cpuId) allIds.cpu.add(config.cpuId)
      if (config.gpuId) allIds.gpu.add(config.gpuId)
      if (config.moboId) allIds.mobo.add(config.moboId)
      if (config.ramId) allIds.ram.add(config.ramId)
      if (config.ssdId) allIds.ssd.add(config.ssdId)
      if (config.pcuId) allIds.pcu.add(config.pcuId)
    })

    // Fetch each component in parallel
    const cpuPromises = Array.from(allIds.cpu).map((id) =>
      fetchComponent('CPU', id, token)
    )
    const gpuPromises = Array.from(allIds.gpu).map((id) =>
      fetchComponent('GPU', id, token)
    )
    const moboPromises = Array.from(allIds.mobo).map((id) =>
      fetchComponent('Mobo', id, token)
    )
    const ramPromises = Array.from(allIds.ram).map((id) =>
      fetchComponent('Ram', id, token)
    )
    const ssdPromises = Array.from(allIds.ssd).map((id) =>
      fetchComponent('SSD', id, token)
    )
    const pcuPromises = Array.from(allIds.pcu).map((id) =>
      fetchComponent('Pcu', id, token)
    )

    await Promise.all([
      ...cpuPromises,
      ...gpuPromises,
      ...moboPromises,
      ...ramPromises,
      ...ssdPromises,
      ...pcuPromises
    ])
  }

  const fetchComponent = async (type, id, token) => {
    const cacheKey = `${type}-${id}`

    // Return cached data if available
    if (componentCache[cacheKey]) {
      console.log(`Cache hit for ${cacheKey}:`, componentCache[cacheKey])
      return componentCache[cacheKey]
    }

    try {
      // map logical type to REST resource name
      const map = {
        CPU: 'cpu',
        GPU: 'gpu',
        Mobo: 'Mobo',
        Ram: 'ram',
        SSD: 'ssd',
        Pcu: 'pcu'
      }

      const resource = map[type]
      if (!resource) return null

      const endpoint = `${API_BASE_URL}/${resource}/${id}`
      console.log(`Fetching ${type} from ${endpoint}`)

      const response = await fetch(endpoint, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      })

      console.log(`Response status for ${cacheKey}: ${response.status}`)

      if (!response.ok) {
        console.error(`Failed to fetch ${type} ${id}: ${response.status} ${response.statusText}`)
        const text = await response.text()
        console.error(`Response body: ${text}`)
        return null
      }

      const text = await response.text()
      if (!text) {
        console.error(`Empty response for ${cacheKey}`)
        return null
      }

      const data = JSON.parse(text)
      console.log(`Fetched ${cacheKey}:`, data)
      componentCache[cacheKey] = data
      return data
    } catch (err) {
      console.error(`Error fetching ${type} ${id}:`, err)
    }

    return null
  }

  const getComponentDisplay = (type, id) => {
    if (!id) return '-'
    
    const cacheKey = `${type}-${id}`
    const component = componentCache[cacheKey]

    if (!component) {
      return `ID: ${id}`
    }

    switch (type) {
      case 'CPU':
        return component.model || component.Model || `ID: ${id}`
      case 'GPU':
        return `${component.brand || component.Brand || ''} ${component.model || component.Model || ''}`.trim() || `ID: ${id}`
      case 'Mobo':
        return `${component.model || component.Model || ''} ${component.chip || component.Chip || ''}`.trim() || `ID: ${id}`
      case 'Ram':
        // RAM displays only model
        return component.model || component.Model || `ID: ${id}`
      case 'SSD':
        return `${component.brand || component.Brand || ''} ${component.model || component.Model || ''}`.trim() || `ID: ${id}`
      case 'Pcu':
        // PCU displays only model
        return component.model || component.Model || `ID: ${id}`
      default:
        return `ID: ${id}`
    }
  }

  const handleLogout = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('username')
    navigate('/')
  }

  const handleDelete = async (configName) => {
    if (!window.confirm('Are you sure you want to delete this configuration?')) return

    try {
      const response = await fetch(`${API_BASE_URL}/config/${configName}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })

      if (!response.ok) {
        throw new Error(`Failed to delete config: ${response.status}`)
      }

      setConfigs(prev => prev.filter(c => c.configName !== configName))
      showNotification('Configuration deleted successfully', 'success')
    } catch (err) {
      showNotification(err.message || 'Failed to delete configuration', 'error')
      console.error('Delete error:', err)
    }
  }

  const handleMore = (componentType, componentId) => {
    if (!componentId) {
      showNotification('Component not available', 'error')
      return
    }

    // Map component type to route
    const typeToRoute = {
      'CPU': 'cpu',
      'GPU': 'gpu',
      'Mobo': 'motherboards',
      'Ram': 'ram',
      'SSD': 'ssd',
      'Pcu': 'pcu'
    }

    const route = typeToRoute[componentType]
    navigate(`/components/${route}/${componentId}`)
  }

  const handleNewConfig = () => {
    setNewConfigForm({
      configName: '',
      purpose: '1080',
      price: '',
      ssdSize: '500'
    })
    setShowCreateModal(true)
  }

  const handleCreateConfig = async (e) => {
    e.preventDefault()

    if (!newConfigForm.configName.trim()) {
      showNotification('Configuration name is required', 'error')
      return
    }

    setCreating(true)

    try {
      const ssdSize = parseInt(newConfigForm.ssdSize)
      const payload = {
        configName: newConfigForm.configName,
        purpose: newConfigForm.purpose,
        price: newConfigForm.price ? parseFloat(newConfigForm.price) : 0
      }

      console.log('Sending config payload:', payload, 'ssdSize:', ssdSize)

      const response = await fetch(`${API_BASE_URL}/config/${ssdSize}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(payload)
      })

      console.log('Config creation response:', response.status, response.statusText)

      if (!response.ok) {
        throw new Error("You're poor lol get a job")
      }

      const newConfig = await response.json()
      setConfigs(prev => [newConfig, ...prev])
      setShowCreateModal(false)
      showNotification('Configuration created successfully', 'success')
    } catch (err) {
      showNotification(err.message || 'Failed to create configuration', 'error')
      console.error('Create error:', err)
    } finally {
      setCreating(false)
    }
  }

  const getComponentName = (type, id) => {
    if (!id) return 'Not selected'
    const key = type.toLowerCase()
    const components = availableComponents[key] || []
    const component = components.find(c => (c.id ?? c.Id ?? c.ID) === id)
    if (!component) return `ID: ${id}`
    return getComponentDisplay(type, id)
  }

  return (
    <div className="configs-page">
      {notification && (
        <div className={`notification notification-${notification.type}`}>
          {notification.message}
        </div>
      )}

      <div className="configs-header">
        <h1>Computer Configurations</h1>
        <div className="header-right">
          <span className="username">{username}</span>
          <button className="btn-logout" onClick={handleLogout}>
            Logout
          </button>
        </div>
      </div>

      <div className="configs-main">
        <div className="configs-topbar">
          <button className="btn-new-config" onClick={handleNewConfig}>
            + New Configuration
          </button>
        </div>

        {loading && (
          <div className="configs-message">Loading configurations...</div>
        )}

        {error && (
          <div className="configs-message error">Error: {error}</div>
        )}

        {!loading && !error && configs.length === 0 && (
          <div className="configs-message">No configurations found. Create your first one!</div>
        )}

        {!loading && !error && configs.length > 0 && (
          <div className="configs-grid">
            {configs.map((config) => (
              <div key={config.configId} className="config-card">
                <div className="card-header">
                  <h2 className="config-name">{config.configName}</h2>
                </div>

                <div className="card-details">
                  <div className="detail-row">
                    <span className="label">Purpose:</span>
                    <span className="value">{config.purpose || 'N/A'}</span>
                  </div>
                  <div className="detail-row">
                    <span className="label">Price:</span>
                    <span className="value">${config.price || '0'}</span>
                  </div>
                </div>

                <div className="card-components">
                  <div className="component-row">
                    <span className="component-label">CPU:</span>
                    <span className="component-value">{getComponentDisplay('CPU', config.cpuId)}</span>
                    <button className="btn-more" onClick={() => handleMore('CPU', config.cpuId)}>More</button>
                  </div>
                  <div className="component-row">
                    <span className="component-label">GPU:</span>
                    <span className="component-value">{getComponentDisplay('GPU', config.gpuId)}</span>
                    <button className="btn-more" onClick={() => handleMore('GPU', config.gpuId)}>More</button>
                  </div>
                  <div className="component-row">
                    <span className="component-label">MOBO:</span>
                    <span className="component-value">{getComponentDisplay('Mobo', config.moboId)}</span>
                    <button className="btn-more" onClick={() => handleMore('Mobo', config.moboId)}>More</button>
                  </div>
                  <div className="component-row">
                    <span className="component-label">RAM:</span>
                    <span className="component-value">{getComponentDisplay('Ram', config.ramId)}</span>
                    <button className="btn-more" onClick={() => handleMore('Ram', config.ramId)}>More</button>
                  </div>
                  <div className="component-row">
                    <span className="component-label">SSD:</span>
                    <span className="component-value">{getComponentDisplay('SSD', config.ssdId)}</span>
                    <button className="btn-more" onClick={() => handleMore('SSD', config.ssdId)}>More</button>
                  </div>
                  <div className="component-row">
                    <span className="component-label">PCU:</span>
                    <span className="component-value">{getComponentDisplay('Pcu', config.pcuId)}</span>
                    <button className="btn-more" onClick={() => handleMore('Pcu', config.pcuId)}>More</button>
                  </div>
                </div>

                <div className="card-footer">
                  <div className="total-price">
                    Total: <span className="price-value">${config.price || '0'}</span>
                  </div>
                  <button className="btn-delete" onClick={() => handleDelete(config.configName)}>
                    Delete
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Create Configuration Modal */}
      {showCreateModal && (
        <div className="modal-overlay" onClick={() => setShowCreateModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h2>Create New Configuration</h2>
              <button className="modal-close" onClick={() => setShowCreateModal(false)}>×</button>
            </div>

            <form onSubmit={handleCreateConfig} className="modal-form">
              <div className="form-group">
                <label>Configuration Name *</label>
                <input
                  type="text"
                  value={newConfigForm.configName}
                  onChange={(e) => setNewConfigForm({...newConfigForm, configName: e.target.value})}
                  placeholder="e.g., Gaming PC"
                  required
                />
              </div>

              <div className="form-group">
                <label>Purpose *</label>
                <select
                  value={newConfigForm.purpose}
                  onChange={(e) => setNewConfigForm({...newConfigForm, purpose: e.target.value})}
                  required
                >
                  <option value="1080">1080p</option>
                  <option value="1440">1440p</option>
                  <option value="2160">2160p (4K)</option>
                </select>
              </div>

              <div className="form-group">
                <label>Price</label>
                <input
                  type="number"
                  step="0.01"
                  value={newConfigForm.price}
                  onChange={(e) => setNewConfigForm({...newConfigForm, price: e.target.value})}
                  placeholder="0.00"
                />
              </div>

              <div className="form-group">
                <label>SSD Size *</label>
                <select
                  value={newConfigForm.ssdSize}
                  onChange={(e) => setNewConfigForm({...newConfigForm, ssdSize: e.target.value})}
                  required
                >
                  <option value="500">500 GB</option>
                  <option value="1000">1000 GB (1 TB)</option>
                  <option value="2000">2000 GB (2 TB)</option>
                </select>
              </div>

              <div className="modal-actions">
                <button type="button" className="btn-cancel" onClick={() => setShowCreateModal(false)}>
                  Cancel
                </button>
                <button type="submit" className="btn-submit" disabled={creating}>
                  {creating ? 'Creating...' : 'Create Configuration'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Configuration Details Modal */}
    </div>
  )
}
