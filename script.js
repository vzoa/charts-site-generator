function openpage(pageName, elmnt) {
    // Hide all elements with class="tabcontent" by default */
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].classList.add("hidden")
    }

    // Set all tabs to inactive except currently clicked one
    tablinks = document.getElementsByClassName("tablink");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].classList.remove("tablink-active")
    }
    elmnt.classList.add("tablink-active")

    // Show the specific tab content
    document.getElementById(pageName).classList.remove("hidden")
}

function openpdf(pdfUrl, elmnt) {
    var viewer = document.getElementById("pdfViewer");

    // Set all charts to inactive except currently clicked one
    tablinks = document.getElementsByClassName("chartbutton");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].classList.remove("chartbutton-active")
    }
    elmnt.classList.add("chartbutton-active")

    // Set PdfViewer
    viewer.setAttribute("data", pdfUrl)
    viewer.classList.remove("hidden")

    // Set parent div background to white
    viewer.parentElement.classList.add("bg-white")
}
