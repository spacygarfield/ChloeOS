let desktopDirectoryId;

$(document).ready(function() {
    // const $desktopDirectory = $.get(`/fs/dir?name="Desktop"`);
    desktopDirectoryId = `AC052FF0-D9DF-4526-B349-42DEFAED77A6`;

    const filePositions = loadFilePositions();

    filePositions?.forEach(position => {
        const fileId = position.id;
        const $file = $(`.file[data-id="${fileId}"]`);
        console.log($file)

        $file.css({ 'grid-column': position.x, 'grid-row': position.y });
    });
});

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
        console.log(`hello!`)

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
