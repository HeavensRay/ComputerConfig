import { NavLink } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import './Navbar.css'

export default function Navbar() {
  const { logout } = useAuth()

  const handleLogout = () => {
    logout()
    window.location.href = '/login'
  }

  return (
    <nav className="navbar">
      <div className="nav-container">
        <div className="nav-links">
          <NavLink
            to="/config"
            className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
          >
            Configurations
          </NavLink>
          
          <NavLink
            to="/components/gpu"
            className={({ isActive }) => `nav-link ${isActive || window.location.pathname.startsWith('/components/gpu') ? 'active' : ''}`}
          >
            GPU
          </NavLink>
          
          <NavLink
            to="/components/cpu"
            className={({ isActive }) => `nav-link ${isActive || window.location.pathname.startsWith('/components/cpu') ? 'active' : ''}`}
          >
            CPU
          </NavLink>
          
          <NavLink
            to="/components/motherboards"
            className={({ isActive }) => `nav-link ${isActive || window.location.pathname.startsWith('/components/motherboards') ? 'active' : ''}`}
          >
            Motherboard
          </NavLink>
          
          <NavLink
            to="/components/ram"
            className={({ isActive }) => `nav-link ${isActive || window.location.pathname.startsWith('/components/ram') ? 'active' : ''}`}
          >
            RAM
          </NavLink>
          
          <NavLink
            to="/components/ssd"
            className={({ isActive }) => `nav-link ${isActive || window.location.pathname.startsWith('/components/ssd') ? 'active' : ''}`}
          >
            SSD
          </NavLink>
          
          <NavLink
            to="/components/pcu"
            className={({ isActive }) => `nav-link ${isActive || window.location.pathname.startsWith('/components/pcu') ? 'active' : ''}`}
          >
            PCU
          </NavLink>
        </div>
        
        <button className="nav-logout" onClick={handleLogout}>
          Logout
        </button>
      </div>
    </nav>
  )
}
