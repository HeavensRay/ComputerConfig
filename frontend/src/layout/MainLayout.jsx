import { Outlet } from 'react-router-dom'
import Navbar from '../components/Navbar'
import './MainLayout.css'

export default function MainLayout() {
  return (
    <div className="main-layout">
      <Navbar />
      <div className="main-content">
        <Outlet />
      </div>
    </div>
  )
}
