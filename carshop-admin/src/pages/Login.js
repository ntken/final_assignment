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

      console.log("Decoded Token:", decodedToken);
      console.log("Role", decodedToken["Role"]);
      if (decodedToken["Role"] !== "Admin") {
        throw new Error("Access denied. Only Admin users can log in.");
      }

      login(token); // Cập nhật context
      console.log("Navigating to Dashboard...");
      navigate("/"); // Điều hướng về Dashboard
    } catch (error) {
      setErrorMessage(error.response?.data?.message || error.message || "Login failed");
    }
  };

  return (
    <div>
      <h2>Login</h2>
      <div>
        <label>Email: </label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>
      <div>
        <label>Password: </label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>
      <button onClick={handleLogin}>Login</button>
      {errorMessage && <p style={{ color: "red" }}>{errorMessage}</p>}
    </div>
  );
};

export default Login;
