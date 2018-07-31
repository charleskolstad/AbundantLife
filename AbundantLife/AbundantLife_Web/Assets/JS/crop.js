let ctx;
let image = new Image();
let scale = 1;
let click = false;
let baseX = 0;
let baseY = 0;
let lastPointX = 0;
let lastPointY = 0;

function setupCanvas(canvas){
    ctx = document.getElementById(canvas).getContext("2d");
}

function onImageLoad() {
    drawImage(0, 0);
    ctx.canvas.onmousedown = onMouseDown.bind(this);
    ctx.canvas.onmousemove = onMouseMove.bind(this);
    ctx.canvas.onmouseup = onMouseUp.bind(this);
    ctx.canvas.onmouseout = onMouseOut.bind(this);
}

function drawImage(x, y) {
    let w = ctx.canvas.width;
    let h = ctx.canvas.height;
    let imgWidth = image.width;
    let imgHeight = image.height;

    let scaleWidth = Math.ceil(imgWidth / (imgWidth / w));
    let scaleHeight = Math.ceil(imgHeight / (imgWidth / w));

    ctx.clearRect(0, 0, w, h);
    baseX = baseX + (x - lastPointX);
    baseY = baseY + (y - lastPointY);
    lastPointX = x;
    lastPointY = y;
    ctx.drawImage(image, baseX, baseY, scaleWidth * scale, scaleHeight * scale);

    drawCutout();
}

function drawCutout() {
    ctx.fillStyle = 'rgba(128, 128, 128, 0.7)';
    ctx.beginPath();
    ctx.rect(0, 0, ctx.canvas.width, ctx.canvas.height);

    ctx.moveTo(20, 20);
    ctx.lineTo(20, 230);
    ctx.lineTo(230, 230);
    ctx.lineTo(230, 20);
    ctx.closePath();
    ctx.fill();
}

function onMouseDown(e) {
    e.preventDefault();

    let loc = windowToCanvas(e.clientX, e.clientY);
    click = true;
    lastPointX = loc.x;
    lastPointY = loc.y;
}

function onMouseMove(e) {
    e.preventDefault();

    if (click) {
        let loc = windowToCanvas(e.clientX, e.clientY);
        drawImage(loc.x, loc.y);
    }
}

function onMouseOut(e) {
    onMouseUp(e);
}

function onMouseUp(e) {
    e.preventDefault();
    click = false;
}

function windowToCanvas(x, y) {
    let canvas = ctx.canvas;
    let bbox = canvas.getBoundingClientRect();
    return {
        x: x - bbox.left * (canvas.width / bbox.width),
        y: y - bbox.top * (canvas.height / bbox.height)
    };
}

function updateScale(e) {
    scale = e.target.value;
    drawImage(lastPointX, lastPointY);
}