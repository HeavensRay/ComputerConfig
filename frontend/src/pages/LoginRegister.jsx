import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import './LoginRegister.css'

const API_BASE_URL = 'http://localhost:5271/api/user'

export default function LoginRegister({ showNotification }) {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()

  const handleLogin = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      const response = await fetch(`${API_BASE_URL}/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
      })

      const data = await response.json()

      if (!response.ok) {
        showNotification(data.message || 'Login failed', 'error')
        return
      }

      localStorage.setItem('token', data.token)
      localStorage.setItem('username', data.username)
      showNotification(`Welcome, ${data.username}!`, 'success')

      setUsername('')
      setPassword('')
      navigate('/config')
    } catch (error) {
      showNotification(error.message || 'An error occurred', 'error')
    } finally {
      setLoading(false)
    }
  }

  const handleRegister = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      const response = await fetch(`${API_BASE_URL}/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
      })

      const data = await response.json()

      if (!response.ok) {
        showNotification(data.message || 'Registration failed', 'error')
        return
      }

      localStorage.setItem('token', data.token)
      localStorage.setItem('username', data.username)
      showNotification(`Welcome, ${data.username}!`, 'success')

      setUsername('')
      setPassword('')
      navigate('/config')
    } catch (error) {
      showNotification(error.message || 'An error occurred', 'error')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="login-register-container">
      <div className="card">
        <h1 className="title">Enter credentials</h1>

        <form className="form">
          <div className="form-group">
            <label htmlFor="username">Username</label>
            <input
              id="username"
              type="text"
              placeholder="Enter your username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              placeholder="Enter your password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              disabled={loading}
            />
          </div>

          <div className="button-group">
            <button
              type="submit"
              className="btn btn-login"
              onClick={handleLogin}
              disabled={loading || !username || !password}
            >
              {loading ? 'Loading...' : 'Login'}
            </button>
            <button
              type="submit"
              className="btn btn-register"
              onClick={handleRegister}
              disabled={loading || !username || !password}
            >
              {loading ? 'Loading...' : 'Register'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
