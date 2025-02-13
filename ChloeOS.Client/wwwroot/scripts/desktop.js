// -----------------------------------------------------------------------
// Selection rectangle.

const $desktopArea = $(`main`);
const $rect = $('#selection-rect');

// const selectionRectangle = {
//     started: false,
//     start: function(e) {
//         // Discard windows.
//         if (e.currentTarget.classList.contains(`window`)) {
//             return;
//         }
//
//         selectionRectangle.started = true;
//
//         // Set start position for selection rectangle.
//         const [ x, y ] = [ e.clientX, e.clientY ];
//         $rect.css({
//             'top': `${x}px`,
//             'left': `${y}px`,
//         }).removeClass(`hidden`);
//     },
//     scale: function(e) {
//         e.stopImmediatePropagation();
//
//         // Don't do anything if the selection rectangle wasn't dragged yet.
//         if (!selectionRectangle.started) {
//             return;
//         }
//
//         const [ startX, startY ] = [ $rect.css(`--rect-x`), $rect.css(`--rect-y`) ];
//         const [ endX, endY ] = [ e.clientX, e.clientY ];
//         const width = endX - startX;
//         const height = endY - startY;
//
//         if (width >= 0) {
//             $rect.css({
//                 'left': `${startX}px`,
//                 'right': null
//             });
//         } else {
//             $rect.css({
//                 'right': `${startX}px`,
//                 'left': null
//             });
//         }
//
//         if (height >= 0) {
//             $rect.css({
//                 'top': `${startY}px`,
//                 'bottom': null
//             });
//         } else {
//             $rect.css({
//                 'bottom': `${startY}px`,
//                 'top': null
//             });
//         }
//
//         $rect.css({
//             '--rect-width': width,
//             '--rect-height': height
//         });
//     },
//     end: function() {
//         selectionRectangle.started = false;
//
//         $rect.css({
//             'top': null,
//             'bottom': null,
//             'left': null,
//             'right': null,
//             '--rect-width': 0,
//             '--rect-height': 0
//         }).addClass(`hidden`);
//     }
// };
//
// $desktopArea.mousedown(selectionRectangle.start);
// $desktopArea.on(`mousemove`, selectionRectangle.scale);
// $desktopArea.mouseup(selectionRectangle.end);

// -----------------------------------------------------------------------
// System clock (in tray).

const $systemClock = $(`#system-clock`);

updateSystemClock();
window.setInterval(updateSystemClock, 1000);

function updateSystemClock() {
    const time = new Date().toLocaleTimeString().toUpperCase();
    $systemClock.text(time);
}

// -----------------------------------------------------------------------
// Window builder.

const windowTemplate = $(`#window-template`)[0];

function openWindow(uri) {
    if (!isRouteAvailable(uri)) {
        uri = `/error/404`;
    }

    uri = window.encodeURIComponent(uri);

    // Create window.
    let $window = $(windowTemplate.content.cloneNode(true));
    let $iframe = $window.children().last();
    $iframe.attr(`src`, uri);

    // Add onto the desktop!
    $desktopArea.append($window);

    $window = $desktopArea.find(`.window`);
    $window.css(`width`, $(document.documentElement).css(`--win-init-width`))
        .css(`height`, $(document.documentElement).css(`--win-init-height`))

    $iframe = $window.children().last();
    $iframe.attr(`src`, uri)
        .attr(`referrerpolicy`, `same-origin`);

    // Verify that the page was loaded successfully.
    // Needs to have at least something in the body.
    if ($iframe[0].contentWindow.location.href === `about:blank`) {
        $iframe.attr(`src`, `/error/404`);
    }
}

function closeWindow($window) {
    $window.remove();
}

let $draggedWindow;
let draggedX;
let draggedY;

function dragStartWindow(e) {
    console.log(`Drag start.`);
    $draggedWindow = $(e.currentTarget);

    const rect = e.currentTarget.getBoundingClientRect();
    draggedX = e.clientX - rect.left;
    draggedY = e.clientY - rect.top;
}

function dragWindow(e) {
    console.log($draggedWindow)
    if (!$draggedWindow) {
        return;
    }

    console.log(`Dragging...`);
    console.log(e.clientX, e.clientY);

    $draggedWindow.css({
        'top': `${e.clientY - draggedY}px`,
        'left': `${e.clientX - draggedX}px`,
    });
}

function dragEndWindow() {
    if (!$draggedWindow) {
        return;
    }

    console.log(`Drag stopped.`);
    $draggedWindow = undefined;
    draggedX = undefined;
    draggedY = undefined;
}

function isRouteAvailable(uri) {
    return $.get(uri, function() {
        return true;
    }).fail(function() {
        return false;
    });
}

// -----------------------------------------------------------------------
// Settings.

$(`[data-settings]`).click(openSettings);

function openSettings() {
    const uri = `/settings/${$(this).attr(`data-settings`)}`;
    if (isSettingsOpen(uri)) {
        return;
    }

    openWindow(uri);
}

function isSettingsOpen(uri) {
    const $window = $(`.window iframe[src="${uri}"]`);
    return !$window;
}