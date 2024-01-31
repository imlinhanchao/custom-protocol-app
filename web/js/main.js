const protocol = "protocol://";
const downloadUrl = "https://github.com/imlinhanchao/custom-protocol-app/releases/download/v1.0.0/setup.exe";

/**
 * @description Custom Protocol Check and Open
 * @param {string} path
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

function connectSocket(onMessage) {
  return new Promise((resolve, reject) => {
    const ws = new WebSocket("ws://localhost:12345");
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
      setTimeout(() => connectSocket(onMessage), 1000);
    };
  });
}

function getAppPath() {
  const path = document.querySelector("#path").value;
  const query = document.querySelector("#query").value;
  return `${path}?${query}`;
}

let sock = null;
document.querySelector("#open").addEventListener("click", () => {
  openProtocol(getAppPath(), () => {
    console.log("Custom Protocol is installed.");
    const result = document.querySelector("#result");
    connectSocket((data) => {
      console.log(data);
      result.value += `${new Date().toLocaleString()} ${JSON.stringify(data)}\n`;
    }).then((ws) => sock = ws);
  });
});
document.querySelector("#ping").addEventListener("click", () => {
  if (!sock) return;
  // WebSocket Ping Pong Command
  sock.send(JSON.stringify({ command: "Ping" }));
});
document.querySelector("#file").addEventListener("change", () => {
  if (!sock) return;
  const file = document.querySelector("#file").files[0];
  const reader = new FileReader();
  // Send File To Custom Path
  reader.onload = (event) => {
    const arrayBuffer = event.target.result;
    const path = document.querySelector("#save").value.replace(/(\\|\/)$/g, "");
    sock.send(JSON.stringify({ command: "Save", data: `${path}\\${file.name}`.replaceAll('/', '\\') }));
    setTimeout(() => sock.send(arrayBuffer), 100);
  };
  reader.readAsArrayBuffer(file);
});
