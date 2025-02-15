const $systemClock = $(`#system-clock`);

updateSystemClock();
window.setInterval(updateSystemClock, 1000);

function updateSystemClock() {
    const time = new Date().toLocaleTimeString().toUpperCase();
    $systemClock.text(time);
}