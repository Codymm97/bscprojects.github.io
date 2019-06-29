function loadNavBar() {
	$("nav").load("../html/nav-bar.html");
}

function showSteps() {
	var steps = document.getElementsByClassName("hidden_1");
	for (var i = 0; i < steps.length; ++i) {
		if (steps[i].style.display == "none") {
			steps[i].style.display = "block";
		} else {
			steps[i].style.display = "none";
		}
	}
}