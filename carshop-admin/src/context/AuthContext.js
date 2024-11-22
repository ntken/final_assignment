import { createContext, useState } from "react";

export const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const [auth, setAuth] = useState(() => {
    const token = localStorage.getItem("token");
    return token ? { isAuthenticated: true, token } : { isAuthenticated: false };
  });

  const login = (token) => {
    localStorage.setItem("token", token);
    setAuth({ isAuthenticated: true, token });
  };

  const logout = () => {
    localStorage.removeItem("token");
    setAuth({ isAuthenticated: false, token: null });
  };

  return (
    <AuthContext.Provider value={{ auth, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;
