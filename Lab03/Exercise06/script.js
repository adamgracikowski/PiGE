const resetBtn = document.querySelector("#resetBtn");
const cells = document.querySelectorAll(".cell");
const gameName = document.querySelector("#gameName");

setInterval(() => {
  gameName.style.textShadow = `0 0.4rem 0.7rem ${randColor()}`;
}, 1000);

resetBtn.addEventListener("click", () => {
  cells.forEach((cell) => (cell.style.backgroundColor = "white"));
});

cells.forEach((cell) => {
  cell.addEventListener("mouseover", setColor);
});

function setColor() {
  this.style.backgroundColor = randColor();
}

function randColor() {
  let red = Math.floor(Math.random() * 256);
  let green = Math.floor(Math.random() * 256);
  let blue = Math.floor(Math.random() * 256);
  return `rgb(${red},${green},${blue})`;
}
