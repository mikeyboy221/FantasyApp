var playerCards = [];
var selectedPlayers = [];

function setPlayerSelected(playerCard) {
    var selectedPlayerId = playerCard.id;
    var selectedRole = playerCard.dataset.role;

    function createPlayerObject(newId, newRole, card) {
        return {
            id: newId,
            role: newRole,
            playerCard: card,
        };
    }

    const existingPlayerIndex = selectedPlayers.findIndex(player => player.role === playerCard.dataset.role);
    if (existingPlayerIndex !== -1) {
        selectedPlayers[existingPlayerIndex].playerCard.classList.remove('active');
        selectedPlayers[existingPlayerIndex] = createPlayerObject(selectedPlayerId, selectedRole, playerCard);
    } else {
        selectedPlayers.push(createPlayerObject(selectedPlayerId, selectedRole, playerCard));
    }

    playerCard.classList.add('active');

    var indicator = document.querySelector('#' + selectedRole + '_FilterButton > #indicator');
    indicator.classList.add('active');

    var selectedOverview = document.getElementById('selected_' + selectedRole + '_overview');
    var selectedTeam = document.getElementById('selected_' + selectedRole + '_team');
    var selectedName = document.getElementById('selected_' + selectedRole + '_name');

    selectedOverview.classList.remove('hidden');
    selectedTeam.innerHTML = playerCard.dataset.team;
    selectedName.innerHTML = playerCard.dataset.name;
}

function initialize() {
    playerCards = Array.from(document.querySelectorAll('#PlayerGrid > div'));
    filterButtons = Array.from(document.querySelectorAll('#FilterButtons > button'));

    function filterPlayerCards(role) {
        playerCards.forEach(function (playerCard) {
            var playerRole = playerCard.dataset.role;

            if (playerRole == role) {
                playerCard.style.display = 'block';
            } else {
                playerCard.style.display = 'none';
            }
        });
    }

    function filterButtonClicked(button) {
        filterButtons.forEach(function (filterButton) {
            if (filterButton.dataset.role == button.dataset.role) {
                filterButton.classList.add('active');
            } else {
                filterButton.classList.remove('active');
            }
        })
    }

    playerCards.forEach(function (playerCard) {
        playerCard.addEventListener('click', function () {
            playerCard.classList.add('active');
            setPlayerSelected(playerCard);
        });
    });

    filterButtons.forEach(function (filterButton) {
        if (filterButton.dataset.role == 'top') {
            filterButton.classList.add('active');
            filterPlayerCards('top');
        }

        filterButton.addEventListener('click', function () {
            filterButtonClicked(filterButton)
            filterPlayerCards(filterButton.dataset.role);
        });
    });

    selectedOverviewCards = Array.from(document.querySelectorAll('#Overview > div'));
    selectedOverviewCards.forEach(function (overviewCard) {
        var removeButton = overviewCard.querySelector('#remove_role');
        removeButton.addEventListener('click', function () {
            var role = overviewCard.dataset.role;

            var indicator = document.querySelector('#' + role + '_FilterButton > #indicator');
            indicator.classList.remove('active');

            var selectedOverview = document.getElementById('selected_' + role + '_overview');
            selectedOverview.classList.add('hidden');

            var selectedPlayer = selectedPlayers.find(player => player.role === role);
            selectedPlayer.playerCard.classList.remove('active');

            var index = selectedPlayers.indexOf(selectedPlayer);
            if (index > -1) {
                selectedPlayers.splice(index, 1);
            }

            console.log(selectedPlayers);
        });
    });

    var createDraftForm = document.getElementById('CreateDraftForm')

    createDraftForm.addEventListener('submit', async function(event) {
        event.preventDefault();

        const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

        var form = selectedPlayers.map(player => {
            return {
                id: player.id,
                role: player.role
            }
        })

        const response = await fetch(createDraftForm.dataset.submitto+'&handler=CreateDraft', {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": antiForgeryToken
            },
            body: JSON.stringify(form)
        })
        if (response.ok) {
            window.location.href = response.url;
        } else {
            console.error('Error: ', response.statusText);
        };
    })
}