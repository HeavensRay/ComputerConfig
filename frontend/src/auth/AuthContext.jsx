import { createContext, useContext, useEffect, useState } from 'react'
import { getToken, getUserRole, logout as authLogout } from './auth'

const AuthContext = createContext()

export function AuthProvider({ children }) {
  const [role, setRole] = useState(null)
  const [loaded, setLoaded] = useState(false)

  // Initial load on mount
  useEffect(() => {
    refreshAuth()
  }, [])

  /**
   * Re-read token and update role state
   */
  function refreshAuth() {
    try {
      const userRole = getUserRole()
      setRole(userRole)
    } catch (err) {
      console.error('Error refreshing auth:', err)
      setRole(null)
    } finally {
      setLoaded(true)
    }
  }

  const isAdmin = role === 'Admin'

  const value = {
    role,
    isAdmin,
    refreshAuth,
    logout: () => {
      authLogout()
      setRole(null)
    }
  }

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  )
}

/**
 * Hook to use auth context
 * @returns {object} Auth context value
 */
export function useAuth() {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider')
  }
  return context
}
