const protocol = "protocol://";
const downloadUrl = "https://github.com/imlinhanchao/custom-protocol-app/releases/download/v1.0.0/setup.exe";

/**
 * @description Custom Protocol Check and Open
 * @param {string} url App Router
 * @param {function} callback Custom Protocol lanuching callback
 */
function openProtocol(url, callback) {
  customProtocolCheck(
    `${protocol}${url}`,
    () => {
      if(!confirm("Custom Protocol is not installed. Do you want to install it?")) return;
      window.open(downloadUrl);
    },
    () => {
      callback();
    }, 5000
  );
}

/**
 * @description Try to connect to the app websocket server
 * @param {function} onMessage Receive message from app
 * @returns {Promise<WebSocket>} WebSocket Object
 */
function connectSocket(onMessage, port) {
  return new Promise((resolve) => {
    const ws = new WebSocket(`ws://localhost:${port}`);
    ws.onopen = () => {
      console.log("Socket is connected.");
      ws.onmessage = (message) => {
        onMessage(message.data);
      };
      ws.onclose = () => {
        console.log("Socket is closed.");
      }
      resolve(ws);
    };
    ws.onclose = () => {
      // retry after 1s
      setTimeout(() => connectSocket(onMessage, port), 1000);
    };
  });
}

/**
 * @description Get App Open Router
 * @returns 
 */
function getAppPath() {
  const path = document.querySelector("#path").value;
  const query = document.querySelector("#query").value;
  return `${path}?${query}`;
}

let sock = null;
document.querySelector("#open").addEventListener("click", () => {
  let port = 12345;
  // if form2, generate random port
  if (document.querySelector("#path").value == "form2") {
    port = Math.ceil(Math.random() * 9999 + 10000);
    document.querySelector("#query").value = `port=${port}`
  }

  openProtocol(getAppPath(), () => {
    console.log("Custom Protocol is installed.");

    const result = document.querySelector("#result");
    connectSocket((data) => {
      console.log(data);
      // get message from app
      result.value += `${new Date().toLocaleString()} ${JSON.stringify(data)}\n`;
    }, port).then((ws) => sock = ws);
  });
});

// Send Ping Command
document.querySelector("#ping").addEventListener("click", () => {
  if (!sock) return;
  // WebSocket Ping Pong Command
  sock.send(JSON.stringify({ command: "Ping" }));
});

// Send File to App
document.querySelector("#file").addEventListener("change", () => {
  if (!sock) return;
  const file = document.querySelector("#file").files[0];
  const reader = new FileReader();

  // Send File To Custom Path
  reader.onload = (event) => {
    const arrayBuffer = event.target.result;

    // send save path to app
    const path = document.querySelector("#save").value.replace(/(\\|\/)$/g, "");
    if (!path) return alert("Please input save path.");
    sock.send(JSON.stringify({ command: "Save", data: `${path}\\${file.name}`.replaceAll('/', '\\') }));

    // send file after 100ms
    setTimeout(() => sock.send(arrayBuffer), 100);
  };
  reader.readAsArrayBuffer(file);
});

document.querySelector("#download").addEventListener("click", () => {
  const url = document.querySelector("#url").value;
  if (!url) return alert("Please input download url.");
  const local = document.querySelector("#save").value.replace(/(\\|\/)$/g, "");
  if (!local) return alert("Please input save path.");
  sock.send(JSON.stringify({ command: "Download", data: { url, local } }));
});
