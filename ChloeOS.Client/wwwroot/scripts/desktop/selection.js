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