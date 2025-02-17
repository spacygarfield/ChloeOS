let desktopDirectoryId;

let $selectedFile;
let $draggedFile;

const $files = $(`.file, .folder`);

$(`#desktop`)
    .on(`mousemove`, dragFile)
    .mouseup(dragEndFile)
    .click(unselectFile);

$(window).resize(function() {
    eachFilePosition(loadFilePositions(), adjustFilePositions);
});

$files.mousedown(dragStartFile);
$files.on(`dblclick`, function(e) {
    const $element = $(e.target);
    if (/(P|INPUT)/g.test($element.prop(`nodeName`))) {
        renameFile($element);
    }
});

$(document).ready(function() {
    $.get(`/fs/dir/name/Desktop`, function([ { id } ]) {
        desktopDirectoryId = id;

        eachFilePosition(loadFilePositions(), adjustFilePositions);
    });
});

function getGridRowCount($grid) {
    return $grid.css(`grid-template-rows`).split(` `).length;
}

function getGridColumnCount($grid) {
    return $grid.css(`grid-template-columns`).split(` `).length;
}

function eachFilePosition(filePositions, callback) {
    filePositions?.forEach((position) => {
        const fileId = position.id;
        const $file = $(`.file[data-id="${fileId}"]`);

        callback(position, $file);
    });
}

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
    $draggedFile.clone = $draggedFile.clone();

    const rect = e.currentTarget.getBoundingClientRect();
    $draggedFile.x = e.clientX - rect.left;
    $draggedFile.y = e.clientY - rect.top;
    $draggedFile.width = rect.width;
    $draggedFile.height = rect.height;

    readjustFile(e.clientX, e.clientY);

    $draggedFile.clone.addClass(`dragged`);
    $(`#desktop`).append($draggedFile.clone);
}

function dragFile(e) {
    if (!$draggedFile) {
        return;
    }

    readjustFile(e.clientX, e.clientY);
}

function readjustFile(x, y) {
    $draggedFile.clone.css({
        'top': y - $draggedFile.y,
        'left': x - $draggedFile.x,
        'width': $draggedFile.width,
        'height': $draggedFile.height
    });
}

function dragEndFile(e) {
    if (!$draggedFile) {
        return;
    }

    const $desktop = $(`#desktop`);
    const padding = Number.parseInt($draggedFile.css(`padding`));

    const x = Math.floor((Math.floor(e.clientX) - (2 * padding)) / $draggedFile.width) + 1;
    const y = Math.floor((Math.floor(e.clientY) - (2 * padding)) / $draggedFile.height) + 1;

    $draggedFile.clone.removeClass(`dragged`);
    $draggedFile.css({ 'grid-row': y, 'grid-column': x });

    const currentPositions = loadFilePositions().forEach((position) => {
        position.x = x;
        position.y = y;
    });

    eachFilePosition(currentPositions, adjustFilePositions);
    saveLayout();
    $draggedFile.clone.remove();
    $draggedFile = undefined;
}

function renameFile($filename) {
    // Convert to text form or back to small.
    const type = $filename.prop("nodeName");
    if (type === `P`) {
        $filename.replaceWith(`
            <div class="ui form">
                <div class="field">
                    <input type="text" class="input" data-window-prevent />
                </div>
            </div>
        `);
        $filename.find(`input`).focus();
    } else if (type === `INPUT`) {
        $filename.prop(`nodeName`, `P`);
    }

}

function loadFilePositions() {
    return JSON.parse(window.localStorage.getItem(`dir:${desktopDirectoryId}`))?.filePositions;
}

function saveLayout() {
    const filePositions = saveFilePositions();
    window.localStorage.setItem(`dir:${desktopDirectoryId}`, JSON.stringify({ filePositions }));
}

function saveFilePositions() {
    return Array.from($(`div.file, div.folder`)).map(file => {
        const $file = $(file);

        let row = Number.parseInt($file.css(`grid-row`));
        if (Number.isNaN(row)) {
            row = 1;
        }

        let column = Number.parseInt($file.css(`grid-column`));
        if (Number.isNaN(column)) {
            column = 1;
        }

        return { id: $file.data(`id`), x: column, y: row };
    });
}

function adjustFilePositions({ id, x, y }, $file) {
    // Check upper bounds.
    const $desktop = $(`#desktop`);

    const rows = getGridRowCount($desktop);
    if (y > rows) {
        y = rows;
    }

    const columns = getGridColumnCount($desktop);
    if (x > columns) {
        x = columns;
    }

    console.log({ x, y, rows, columns })
    $file.css({ 'grid-column': x, 'grid-row': y });
}