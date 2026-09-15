import axios from "axios";
import secureLocalStorage from "react-secure-storage";

const getBaseUrl = () => {
  if (process.env.NEXT_PUBLIC_API_URL) {
    return process.env.NEXT_PUBLIC_API_URL.endsWith("/")
      ? process.env.NEXT_PUBLIC_API_URL
      : `${process.env.NEXT_PUBLIC_API_URL}/`;
  }
  return "http://localhost:5000/api/";
};

export const api = axios.create({
  baseURL: getBaseUrl(),
});

// Interceptor para anexar o token JWT nas requisições
api.interceptors.request.use((config) => {
  if (typeof window !== "undefined") {
    try {
      const token = secureLocalStorage.getItem("Token");
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
    } catch {
      // Ignora erro em SSR
    }
  }
  return config;
});

// Utilitário para resolver URL de imagem retornada pela API
export const resolveImageUrl = (url?: string | null): string => {
  if (!url) return "/imgs/negona_robotica.png";
  if (url.startsWith("http://") || url.startsWith("https://") || url.startsWith("data:")) {
    return url;
  }
  const base = api.defaults.baseURL || "http://localhost:5000/api/";
  const cleanBase = base.endsWith("/") ? base : `${base}/`;
  const cleanUrl = url.startsWith("/") ? url.slice(1) : url;
  return `${cleanBase}${cleanUrl}`;
};
