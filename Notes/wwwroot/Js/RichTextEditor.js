window.getEditorHtml = (element) => {
   
    return element.innerHTML.replace(/<!--[\s\S]*?-->/g, "");
};

window.setEditorHtml = function (element, html) {
    element.innerHTML = html;
};

window.performAction = function (editor, command, value) {

    document.execCommand(command, false, value);
    window.focusEditor(editor);
};

window.focusEditor = function (editor) {
    editor.focus();
};

window.placeCaretToEnd = function (editor) {

    let range = document.createRange();
    range.selectNodeContents(editor);
    range.collapse(false); // Move caret to end
    selection.removeAllRanges();
    selection.addRange(range);
};


let optionsButtons = document.querySelectorAll(".option-button");
let advancedOptionButton = document.querySelectorAll(".adv-option-button");
let fontName = document.getElementById("fontName");
let fontSize = document.getElementById("fontSize");
let writingArea = document.getElementById("text-input");
let linkButton = document.getElementById("createLink");
let alignButtons = document.querySelectorAll(".align");
let spacingButtons = document.querySelectorAll(".spacing");
let formatButtons = document.querySelectorAll(".format");
let scriptButtons = document.querySelectorAll(".script");

let fontList = ["Arial", "Verdana", "Times New Roman", "Garamond", "Georgia", "Courier New", "Cursive"];



const initializer = () => {

    highlighter(alignButtons, true);
    highlighter(spacingButtons, true);
    highlighter(formatButtons, false);
    highlighter(scriptButtons, true);

    console.log(alignButtons);
    console.log(spacingButtons);
    console.log(formatButtons);
    console.log(scriptButtons);
};


const highlighter = (className, needsRemoval) => {
    className.forEach((button) => {
        button.addEventListener("click", () => {

            console.log("click runned");

            if (needsRemoval) {
                let alreadyActive = false;

                if (button.classList.contains("active")) {
                    alreadyActive = true;
                }

                highlighterRemover(className);

                if (!alreadyActive) {
                    button.classList.add("active");
                }
            }
            else {
                button.classList.toggle("active");
            }

        });
    });
};

window.onload = initializer();







