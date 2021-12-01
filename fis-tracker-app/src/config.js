let config;

if (process.env.NODE_ENV === "production") {
  config = {
    apiUrl: "",
    timeoutDuration: 30000
  };
} else {
  config = {
    apiUrl: "https://localhost:5100/api",
    timeoutDuration: 1000
  };
}

export default config