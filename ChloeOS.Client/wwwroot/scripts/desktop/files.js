let $selectedFile;
let $draggedFile;

$(`#desktop`)
    .on(`mousemove`, dragFile)
    .mouseup(dragEndFile)
    .click(unselectFile);

$(`.file, .folder`).mousedown(dragStartFile);

// function selectFile(e) {
//     e.stopPropagation();
//
//     if ($selectedFile) {
//         return;
//     }
//
//     $selectedFile = $(e.currentTarget);
//     $selectedFile.addClass(`selected`);
// }
//
function unselectFile(e) {
    const $selected = $(e.currentTarget);

    if (!$selectedFile || $selected.is($selectedFile) || $selected.hasClass(`file`)) {
        return;
    }

    $selectedFile.removeClass(`selected`);
    $selectedFile = undefined;
}

function dragStartFile(e) {
    e.stopPropagation();

    if ($draggedFile) {
        return;
    }

    $draggedFile = $(e.currentTarget);

    const rect = e.currentTarget.getBoundingClientRect();
    $draggedFile.x = e.clientX - rect.left;
    $draggedFile.y = e.clientY - rect.top;
    $draggedFile.width = rect.width;
    $draggedFile.height = rect.height;

    console.log(`Dragging file`, $draggedFile);
    $draggedFile.addClass(`dragged`);
}

function dragFile(e) {
    if (!$draggedFile) {
        return;
    }

    console.log(`Dragging file`);
    $draggedFile.css({
        'top': e.clientY - $draggedFile.y,
        'left': e.clientX - $draggedFile.x,
        'width': $draggedFile.width,
        'height': $draggedFile.height
    });
}

function dragEndFile(e) {
    if (!$draggedFile) {
        return;
    }

    let [x, y] = [Math.floor(e.clientX), Math.floor(e.clientY)];

    console.log(`Dropped file`, $draggedFile);
    console.log(`Dropped at`, {x,y});

    const $desktop = $(`#desktop`);
    const padding = Number.parseInt($draggedFile.css(`padding`));

    x = Math.floor((x - (2 * padding)) / $draggedFile.width) + 1;
    y = Math.floor((y - (2 * padding)) / $draggedFile.height) + 1;
    console.log(`Dropped at (relative to grid)`, {x,y});

    $draggedFile.removeClass(`dragged`);
    $draggedFile.css({ 'grid-row': y, 'grid-column': x });

    saveLayout();
    $draggedFile = undefined;
}