import { jwtDecode } from "jwt-decode";
import { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { AuthContext } from "../context/AuthContext";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const navigate = useNavigate();
  const { login } = useContext(AuthContext); // Sử dụng AuthContext

  const handleLogin = async () => {
    try {
      const response = await axios.post("http://localhost:5237/users/login", {
        email,
        password,
      });

      const token = response.data.token;
      const decodedToken = jwtDecode(token);

      if (decodedToken["Role"] !== "Admin") {
        throw new Error("Access denied. Only Admin users can log in.");
      }

      login(token); // Cập nhật context
      navigate("/"); // Điều hướng về Dashboard
    } catch (error) {
      setErrorMessage(error.response?.data?.message || error.message || "Login failed");
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <form onSubmit={(e) => e.preventDefault()}>
        {errorMessage && <p className="error-message">{errorMessage}</p>}
        <div>
          <label>Email:</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="Enter your email"
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Enter your password"
          />
        </div>
        <button className="btn-login" onClick={handleLogin}>
          Login
        </button>
      </form>
    </div>
  );
};

export default Login;
