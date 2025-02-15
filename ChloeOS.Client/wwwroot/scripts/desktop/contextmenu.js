$(document.body).on(`contextmenu`, openContextMenu);

function openContextMenu(e) {
    e.preventDefault();
    console.log(`Opened context menu`);
}