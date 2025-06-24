import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../api';
import './LoginPage.css';

interface Props {
  onLogin: (token: string) => void;
}

export function LoginPage({ onLogin }: Props) {
  useEffect(() => {
    document.title = 'Login';
  }, []);
  
  const [email, setEmail] = useState('');
  const [pwd, setPwd] = useState('');
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const { token } = await login(email, pwd);
      localStorage.setItem('token', token);
      onLogin(token);             
      navigate('/dashboard');     
    } catch {
      setError('Invalid credentials');
    }
  };

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleSubmit}>
        <h1>Login</h1>
        {error && <div className="error">{error}</div>}
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={e => setEmail(e.target.value)}
        />
        <input
          type="password"
          placeholder="Password"
          value={pwd}
          onChange={e => setPwd(e.target.value)}
        />
        <button type="submit">Sign In</button>
      </form>
    </div>
  );
}