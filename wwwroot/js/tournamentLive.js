const controller = new AbortController();

var currentExpandedMatchDiv = null

function showExpandedGame(button, div) {
    button.classList.add('active');
    div.classList.remove('hidden');
}

function hideExpandedGame(button, div) {
    button.classList.remove('active');
    div.classList.add('hidden');
}

async function expandMatchResults(matchId) {
    controller.abort();

    // Constructs url from current location + the handler for the method in Tournament page model
    var url = window.location.href + "&handler=ExpandMatch&matchId=" + matchId;

    fetch(url)
        .then(response => {
            return response.text();
        })
        .then((result) => {
            var matchResultsDiv = document.getElementById(matchId);

            // Handle previously expanded
            if (currentExpandedMatchDiv == matchResultsDiv) {
                // Match is already expanded
                console.log("Match is already expanded");
                return;
            }
            if (currentExpandedMatchDiv != null) {
                currentExpandedMatchDiv.classList.add("hidden");
            }

            // Expand match
            if (matchResultsDiv == null) {
                // Could not find div with matchId
                console.log("Could not find div with matchId: " + matchId);
                return;
            }


            matchResultsDiv.innerHTML = result;
            matchResultsDiv.classList.remove("hidden");

            currentExpandedMatchDiv = matchResultsDiv;

            var buttons = Array.from(currentExpandedMatchDiv.getElementsByTagName("button"));
            var scoreboards = Array.from(currentExpandedMatchDiv.getElementsByClassName("game-scoreboard"));

            buttons.forEach(button => {
                var gameId = button.dataset.gameid;

                button.addEventListener("click", function () {
                    for (i=0; i < buttons.length; i++) {
                        var button = buttons[i];
                        var scoreboard = scoreboards[i];

                        if (scoreboard.id == gameId) {
                            showExpandedGame(button, scoreboard);
                        } else {
                            hideExpandedGame(button, scoreboard);
                        }
                    }
                });
            });

            showExpandedGame(buttons[0], scoreboards[0])
        })
        .catch(error => {
            console.error('Error fetching data:', error);
        });
}

function initialize()
{
    console.log("Tournament Live JS Loaded");
}