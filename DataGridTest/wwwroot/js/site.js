window.myScrollTextArea = function () {
    const textarea = document.getElementById("mock-console");
    console.log(textarea);
    textarea.scrollTop = textarea.scrollHeight;
}

window.setMyScrollTextArea = function () {
    setTimeout(myScrollTextArea, 20);
}