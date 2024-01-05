const PROXY_CONFIG = [
  {
    context: [
      "/loading",
    ],
    target: "https://localhost:7294",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
