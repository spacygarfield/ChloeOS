$(document.body)
    .on(`mousemove`, dragWindow)
    .mouseup(dragEndWindow);

$(`[data-window-click]`).click(createWindow);
$(`[data-window-dblclick]`).on(`dblclick`, createWindow);

const windowTemplate = $(`#window-template`)[0];

function createWindow(e) {
    const $clickedElement = $(e.currentTarget);

    let $window = $(windowTemplate.content.firstElementChild.cloneNode(true));

    // Set custom Window settings.
    setWindowSettings($window, {
        width: !Number.isNaN($clickedElement.data(`window-width`))
            ? $clickedElement.data(`window-width`)
            : $(document.documentElement).css(`--win-init-width`),
        height: !Number.isNaN($clickedElement.data(`window-height`))
            ? $clickedElement.data(`window-height`)
            : $(document.documentElement).css(`--win-init-height`),
        maximized: $clickedElement.data(`window-maximized`) !== undefined,
        resizable: $clickedElement.data(`window-resizable`),
        minimizable: $clickedElement.data(`window-minimizable`),
        maximizable: $clickedElement.data(`window-maximizable`),
        confirmOnExit: $clickedElement.data(`window-confirm-exit`) !== undefined,
    });

    openWindow($window, $clickedElement.data(`window-href`));
}

function openWindow($window, uri) {
    if (!isRouteAvailable(uri)) {
        uri = `/error/404`;
    }

    // Encode query parameters.
    const [href, queryParams] = uri.split(`?`);
    if (queryParams) {
        uri = href + window.encodeURIComponent(queryParams);
    }

    let $iframe = $window.find(`iframe`);
    $iframe.attr({ 'src': uri, 'referrerpolicy': `same-origin` });

    $desktopArea.append($window);

    // Verify that the page was loaded successfully.
    // Needs to have at least something in the body.
    if ($iframe[0].contentWindow.location.href === `about:blank`) {
        $iframe.attr(`src`, `/error/404`);
    }
}

function closeWindow($window) {
    if (Boolean($window.data(`confirm-exit`))) {
        const ok = window.confirm(`Are you sure you want to close this window?`);
        if (ok) {
            $window.remove();
        }
    } else {
        $window.remove();
    }
}

let $draggedWindow;

function dragStartWindow(e) {
    $draggedWindow = $(e.currentTarget);
    $draggedWindow.css(`z-index`, 10);

    const rect = e.currentTarget.getBoundingClientRect();
    $draggedWindow.x = e.clientX - rect.left;
    $draggedWindow.y = e.clientY - rect.top;
}

function dragWindow(e) {
    if (!$draggedWindow) {
        return;
    }

    $draggedWindow.css({
        'top': e.clientY - $draggedWindow.y,
        'left': e.clientX - $draggedWindow.x,
    });
}

function dragEndWindow() {
    if (!$draggedWindow) {
        return;
    }

    $draggedWindow.css(`z-index`, +$($draggedWindow.css(`z-index`)) - 1);
    $draggedWindow = undefined;
}

function isRouteAvailable(uri) {
    return $.get(uri, function() {
        return true;
    }).fail(function() {
        return false;
    });
}

function setWindowSettings($window, {
    width = $window.css(`--win-init-width`),
    height = $window.css(`--win-init-height`),
    maximized = false,
    resizable = false,
    minimizable = true,
    maximizable = true,
    confirmOnExit = false
}) {
    // Check for all the settings.
    $window.css({ width, height });

    if (maximized) {
        const $desktop = $(`#desktop`);
        $window.css({
            'width': $desktop.css(`width`),
            'height': $desktop.css(`height`),
            'top': 0,
            'left': 0
        });
    }

    if (resizable) {
        $window.css(`resizable`, `both`);
    }

    if (!minimizable) {
        $window.find(`.minimize`).prop(`disabled`, true);
    }

    if (!maximizable) {
        $window.find(`.maximize`).prop(`disabled`, true);
    }

    if (confirmOnExit) {
        $window.data(`confirm-exit`, true);
    }
}