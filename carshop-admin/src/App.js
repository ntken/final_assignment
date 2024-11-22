import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Dashboard from "./pages/Dashboard";
import ManageCars from "./pages/ManageCars";
import ManageCompanies from "./pages/ManageCompanies";
import ManageModels from "./pages/ManageModels";
import ManageColors from "./pages/ManageColors";
import ManageUsers from "./pages/ManageUsers";
import Login from "./pages/Login";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/manage-cars" element={<ManageCars />} />
        <Route path="/manage-companies" element={<ManageCompanies />} />
        <Route path="/manage-models" element={<ManageModels />} />
        <Route path="/manage-colors" element={<ManageColors />} />
        <Route path="/manage-users" element={<ManageUsers />} />
        <Route path="/login" element={<Login />} />
      </Routes>
    </Router>
  );
}

export default App;
