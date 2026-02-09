import { Routes, Route } from 'react-router-dom'
import { useState } from 'react'
import LoginRegister from './pages/LoginRegister'
import Configs from './pages/Configs'
import MainLayout from './layout/MainLayout'
import ComponentsLayout from './pages/components/ComponentsLayout'
import ComponentList from './pages/components/ComponentList'
import ComponentDetail from './pages/components/ComponentDetail'
import Toast from './components/Toast'
import './App.css'

export default function App() {
  const [notification, setNotification] = useState(null)

  const showNotification = (message, type = 'success') => {
    setNotification({ message, type })
    setTimeout(() => setNotification(null), 3000)
  }

  return (
    <div className="app">
      {notification && <Toast message={notification.message} type={notification.type} />}
      <Routes>
        <Route path="/" element={<LoginRegister showNotification={showNotification} />} />
        <Route path="/login" element={<LoginRegister showNotification={showNotification} />} />
        
        <Route element={<MainLayout />}>
          <Route path="/config" element={<Configs />} />
            
            <Route path="/components" element={<ComponentsLayout />}>
              <Route path="gpu" element={<ComponentList resource={"GPU"} displayName={"GPU"} />} />
              <Route path="gpu/:id" element={<ComponentDetail resource={"GPU"} displayName={"GPU"} />} />

              <Route path="cpu" element={<ComponentList resource={"CPU"} displayName={"CPU"} />} />
              <Route path="cpu/:id" element={<ComponentDetail resource={"CPU"} displayName={"CPU"} />} />
              
              <Route path="motherboards" element={<ComponentList resource={"Mobo"} displayName={"Motherboards"} />} />
              <Route path="motherboards/:id" element={<ComponentDetail resource={"Mobo"} displayName={"Motherboards"} />} />

              <Route path="ram" element={<ComponentList resource={"Ram"} displayName={"RAM"} />} />
              <Route path="ram/:id" element={<ComponentDetail resource={"Ram"} displayName={"RAM"} />} />

              <Route path="ssd" element={<ComponentList resource={"SSD"} displayName={"SSD"} />} />
              <Route path="ssd/:id" element={<ComponentDetail resource={"SSD"} displayName={"SSD"} />} />

              <Route path="pcu" element={<ComponentList resource={"Pcu"} displayName={"PCU"} />} />
              <Route path="pcu/:id" element={<ComponentDetail resource={"Pcu"} displayName={"PCU"} />} />
            </Route>
          </Route>
        </Routes>
      </div>
  )
}
