function onTournamentChanged() {
    var selectedTournament = document.getElementById("SelectedTournament");
    var selectedTournamentId = selectedTournament.value;

    var beginAt = selectedTournament.options[selectedTournament.selectedIndex].dataset.begin_at;
    var endAt = selectedTournament.options[selectedTournament.selectedIndex].dataset.end_at;

    var TournamentBeginAt = document.getElementById("TournamentBeginAt");
    var TournamentEndAt = document.getElementById("TournamentEndAt");

    TournamentBeginAt.innerHTML = beginAt;
    TournamentEndAt.innerHTML = endAt;
}