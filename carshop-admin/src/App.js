import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { useContext } from "react";
import Dashboard from "./pages/Dashboard";
import ManageCars from "./pages/ManageCars";
import ManageCompanies from "./pages/ManageCompanies";
import ManageModels from "./pages/ManageModels";
import ManageColors from "./pages/ManageColors";
import ManageUsers from "./pages/ManageUsers";
import Login from "./pages/Login";
import { AuthContext, AuthProvider } from "./context/AuthContext";

const AppRoutes = () => {
  const { isLoggedIn } = useContext(AuthContext);

  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      {isLoggedIn ? (
        <>
          <Route path="/" element={<Dashboard />} />
          <Route path="/manage-cars" element={<ManageCars />} />
          <Route path="/manage-companies" element={<ManageCompanies />} />
          <Route path="/manage-models" element={<ManageModels />} />
          <Route path="/manage-colors" element={<ManageColors />} />
          <Route path="/manage-users" element={<ManageUsers />} />
        </>
      ) : (
        <Route path="*" element={<Navigate to="/login" />} />
      )}
    </Routes>
  );
};

const App = () => {
  return (
    <AuthProvider>
      <Router>
        <AppRoutes />
      </Router>
    </AuthProvider>
  );
};

export default App;
