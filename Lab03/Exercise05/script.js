const h1 = document.querySelector("h1");
const tube = document.querySelector("#tube");
const myCanvas = document.querySelector("#myCanvas");

drawLogo();

h1.onmouseover = function () {
  tube.classList.add("oldStyle");
};

h1.onmouseout = function () {
  tube.classList.remove("oldStyle");
};

myCanvas.onclick = function () {
  window.open("https://youtube.com");
};

myCanvas.onmousedown = function () {
  myCanvas.classList.add("clicked");
};

myCanvas.onmouseup = function () {
  myCanvas.classList.remove("clicked");
};

function drawLogo() {
  const ctx = myCanvas.getContext("2d");
  ctx.fillStyle = "red";
  ctx.fillRect(0, 0, 420, 300);

  ctx.strokeStyle = "white";
  ctx.fillStyle = "white";
  ctx.moveTo(140, 70);
  ctx.lineTo(140, 230);
  ctx.lineTo(280, 150);
  ctx.lineTo(140, 70);
  ctx.fill();
  ctx.stroke();
}
