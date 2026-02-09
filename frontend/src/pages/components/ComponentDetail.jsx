import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { getToken } from '../../auth/auth'
import { useAuth } from '../../auth/AuthContext'
import './ComponentDetail.css'

const API_BASE_URL = 'http://localhost:5271/api'

export default function ComponentDetail({ resource, displayName }) {
  const { id } = useParams()
  const navigate = useNavigate()
  const { isAdmin } = useAuth()
  const token = getToken()

  const [component, setComponent] = useState(null)
  const [comments, setComments] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [newComment, setNewComment] = useState('')
  const [submittingComment, setSubmittingComment] = useState(false)

  useEffect(() => {
    fetchComponent()
    fetchComments()
  }, [id, resource])

  const fetchComponent = async () => {
    try {
      const resourceMap = {
        'CPU': 'cpu',
        'GPU': 'gpu',
        'Mobo': 'Mobo',
        'Ram': 'ram',
        'SSD': 'ssd',
        'Pcu': 'pcu'
      }

      const endpoint = resourceMap[resource]
      if (!endpoint) throw new Error('Invalid resource type')

      const response = await fetch(`${API_BASE_URL}/${endpoint}/${id}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          ...(token ? { Authorization: `Bearer ${token}` } : {})
        }
      })

      if (!response.ok) {
        throw new Error(`Failed to fetch component: ${response.status}`)
      }

      const data = await response.json()
      setComponent(data)
    } catch (err) {
      setError(err.message)
      console.error('Error fetching component:', err)
    } finally {
      setLoading(false)
    }
  }

  const fetchComments = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/Comments/${id}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          ...(token ? { Authorization: `Bearer ${token}` } : {})
        }
      })

      if (response.ok) {
        const data = await response.json()
        setComments(Array.isArray(data) ? data : [])
      }
    } catch (err) {
      console.error('Error fetching comments:', err)
    }
  }

  const handleSubmitComment = async (e) => {
    e.preventDefault()
    if (!newComment.trim()) return

    setSubmittingComment(true)
    try {
      const url = new URL(`${API_BASE_URL}/Comments/${id}`)
      url.searchParams.append('writing', newComment)

      const response = await fetch(url.toString(), {
        method: 'POST',
        headers: {
          ...(token ? { Authorization: `Bearer ${token}` } : {})
        }
      })

      if (!response.ok) {
        throw new Error(`Failed to post comment: ${response.status}`)
      }

      setNewComment('')
      await fetchComments()
    } catch (err) {
      console.error('Error posting comment:', err)
    } finally {
      setSubmittingComment(false)
    }
  }

  const handleDeleteComment = async (commentId) => {
    if (!window.confirm('Delete this comment?')) return

    try {
      const response = await fetch(`${API_BASE_URL}/Comments/${commentId}`, {
        method: 'DELETE',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })

      if (!response.ok) {
        throw new Error(`Failed to delete comment: ${response.status}`)
      }

      // Refresh comments list
      await fetchComments()
    } catch (err) {
      console.error('Error deleting comment:', err)
      alert('Failed to delete comment: ' + err.message)
    }
  }

  if (loading) {
    return (
      <div className="component-detail-page">
        <div className="detail-container">
          <div className="loading">Loading {displayName}...</div>
        </div>
      </div>
    )
  }

  if (error || !component) {
    return (
      <div className="component-detail-page">
        <div className="detail-container">
          <div className="error-message">
            {error || 'Component not found'}
          </div>
          <button className="btn-back" onClick={() => navigate(-1)}>
            ← Back
          </button>
        </div>
      </div>
    )
  }

  const model = component.model || component.Model || 'Unknown'
  const brand = component.brand || component.Brand || ''
  const chip = component.chip || component.Chip || ''
  const price = component.price || component.Price || 'N/A'

  return (
    <div className="component-detail-page">
      <div className="detail-container">
        <button className="btn-back" onClick={() => navigate(-1)}>
          ← Back
        </button>

        <div className="detail-card">
          <div className="detail-header">
            <h1>{displayName}: {model}</h1>
          </div>

          <div className="detail-content">
            <div className="detail-info">
              {component && Object.entries(component).map(([key, value]) => {
                // Skip photo, id fields, null values, and undefined
                if (key.toLowerCase().includes('id') || key.toLowerCase() === 'photo' || value === null || value === undefined || value === '') {
                  return null
                }

                // Format the key for display
                const displayKey = key
                  .replace(/([A-Z])/g, ' $1')
                  .replace(/^./, str => str.toUpperCase())
                  .trim()

                return (
                  <div key={key} className="info-item">
                    <span className="info-label">{displayKey}:</span>
                    <span className="info-value">
                      {typeof value === 'object' ? JSON.stringify(value) : value.toString()}
                    </span>
                  </div>
                )
              })}
            </div>
          </div>
        </div>

        <div className="comments-section">
          <h2>Comments ({comments.length})</h2>

          {token && (
            <form onSubmit={handleSubmitComment} className="comment-form">
              <textarea
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
                placeholder="Add a comment..."
                disabled={submittingComment}
              />
              <button type="submit" disabled={!newComment.trim() || submittingComment}>
                {submittingComment ? 'Posting...' : 'Post Comment'}
              </button>
            </form>
          )}

          <div className="comments-list">
            {comments.length === 0 ? (
              <div className="no-comments">No comments yet. Be the first to comment!</div>
            ) : (
              comments.map((comment) => (
                <div key={comment.id} className="comment-item">
                  <div className="comment-header">
                    <span className="comment-username">{comment.username}</span>
                    <span className="comment-date">
                      {comment.createdOn}
                    </span>
                  </div>
                  <p className="comment-text">{comment.writing}</p>
                  {isAdmin && (
                    <button
                      className="btn-delete-comment"
                      onClick={() => handleDeleteComment(comment.id)}
                    >
                      Delete
                    </button>
                  )}
                </div>
              ))
            )}
          </div>
        </div>
      </div>
    </div>
  )
}
