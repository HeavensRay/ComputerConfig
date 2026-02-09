/**
 * JWT token utilities
 * Decodes JWT payload without verification (frontend UX only)
 * Backend enforces actual authorization
 */

const TOKEN_KEY = 'token'

/**
 * Get token from localStorage
 * @returns {string|null} JWT token or null if not found
 */
export function getToken() {
  try {
    return localStorage.getItem(TOKEN_KEY)
  } catch (err) {
    console.error('Error reading token:', err)
    return null
  }
}

/**
 * Decode JWT payload using atob
 * @param {string} token JWT token
 * @returns {object|null} Decoded payload or null if invalid
 */
function decodeToken(token) {
  if (!token || typeof token !== 'string') {
    return null
  }

  try {
    // JWT format: header.payload.signature
    const parts = token.split('.')
    if (parts.length !== 3) {
      return null
    }

    // Decode payload (second part)
    const decoded = atob(parts[1])
    return JSON.parse(decoded)
  } catch (err) {
    console.error('Error decoding token:', err)
    return null
  }
}

/**
 * Get user role from token
 * @returns {string|null} Role string ("Admin", "User") or null if not found
 */
export function getUserRole() {
  const token = getToken()
  if (!token) {
    return null
  }

  const payload = decodeToken(token)
  if (!payload || !payload.role) {
    return null
  }

  return payload.role
}

/**
 * Check if user is admin
 * @returns {boolean} True only if role is "Admin"
 */
export function isAdmin() {
  return getUserRole() === 'Admin'
}

/**
 * Remove token from localStorage
 */
export function logout() {
  try {
    localStorage.removeItem(TOKEN_KEY)
  } catch (err) {
    console.error('Error removing token:', err)
  }
}
