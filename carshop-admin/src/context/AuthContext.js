import { createContext, useState } from "react";
import { jwtDecode } from "jwt-decode";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const token = localStorage.getItem("token");
  const [isLoggedIn, setIsLoggedIn] = useState(!!token && jwtDecode(token)["Role"] === "Admin");

  const login = (token) => {
    const decodedToken = jwtDecode(token);
    //console.log("Decoded Token Auth:", decodedToken);
    //console.log("Role Auth", decodedToken["Role"]);
    if (decodedToken["Role"] !== "Admin") {
      throw new Error("Access denied. Only Admin users can log in.");
    }
    localStorage.setItem("token", token);
    setIsLoggedIn(true);
  };

  const logout = () => {
    localStorage.removeItem("token");
    setIsLoggedIn(false);
  };

  return (
    <AuthContext.Provider value={{ isLoggedIn, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
