const $welcome = $(`#welcome`);

if (window.sessionStorage.getItem(`welcomed`)) {
    $welcome.remove();
} else {
    const startupSound = new Audio(`../sounds/syson.wav`);
    startupSound.play();
    $welcome.on(`animationend`, function () {
        window.sessionStorage.setItem(`welcomed`, `true`);
        $(this).remove();
    });
}