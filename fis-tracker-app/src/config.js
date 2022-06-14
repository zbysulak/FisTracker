let config

if (process.env.NODE_ENV === "production") {
  config = {
    apiUrl: "",
    timeoutDuration: 30000
  }
} else {
  config = {
    //apiUrl: "http://192.168.1.242:5100/api",
    apiUrl: "https://fainee.com/api",
    //apiUrl: "http://localhost:39559/api",
    timeoutDuration: 1000
  }
}

export default config
