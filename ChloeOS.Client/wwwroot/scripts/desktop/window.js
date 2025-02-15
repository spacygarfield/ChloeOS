$(document.body)
    .on(`mousemove`, dragWindow)
    .mouseup(dragEndWindow);

const windowTemplate = $(`#window-template`)[0];

function openWindow(uri) {
    if (!isRouteAvailable(uri)) {
        uri = `/error/404`;
    }
    uri = `/`;

    // Encode query parameters.
    const [href, queryParams] = uri.split(`?`);
    if (queryParams) {
        uri = href + window.encodeURIComponent(queryParams);
    }

    // Create window.
    let $window = $(windowTemplate.content.cloneNode(true));
    let $iframe = $window.find(`iframe`);
    $iframe.attr(`src`, uri);

    // Add onto the desktop!
    $desktopArea.append($window);

    $window = $desktopArea.find(`.window`);
    $window
        .css(`width`, $(document.documentElement).css(`--win-init-width`))
        .css(`height`, $(document.documentElement).css(`--win-init-height`))

    $iframe = $window.find(`iframe`);
    $iframe
        .attr(`src`, uri)
        .attr(`referrerpolicy`, `same-origin`);

    // Verify that the page was loaded successfully.
    // Needs to have at least something in the body.
    // if ($iframe[0].contentWindow.location.href === `about:blank`) {
    //     $iframe.attr(`src`, `/error/404`);
    // }
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