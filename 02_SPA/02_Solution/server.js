/**
 * Created by mstolze on 04/01/17.
 */

const express = require('express');
const path = require('path');
const session = require('express-session');
const expressHandlebars = require('express-handlebars');

const server = express();

server.use(session({	secret: 'gh45sdfgh3asd45df' }));
server.engine('handlebars', expressHandlebars());
server.set('view engine', 'handlebars');

server.use('/', express.static(path.join(__dirname, 'public')));


let spaCounterRaceParticipantsCounter = 0;
const spaTeamNamesArray = ['Bulls', 'Tigers'];
const spaTeamCountsArray = [0, 0];

function getSpaCounterRaceTeamNr (session) {
	if (session.spaCounterRaceTeamNr === undefined) {
		spaCounterRaceParticipantsCounter++;
		session.spaCounterRaceTeamNr = (spaCounterRaceParticipantsCounter % spaTeamNamesArray.length);
	}
	return session.spaCounterRaceTeamNr;
}
function getSpaCounterRaceTeamName (session) {
	return spaTeamNamesArray[getSpaCounterRaceTeamNr(session)];
}
function getSpaCounterTeamCount (session) {
	return spaTeamCountsArray[getSpaCounterRaceTeamNr(session)];
}
function incSpaCounterRaceTeamCount (session) {
	const teamNr = getSpaCounterRaceTeamNr(session);
	spaTeamCountsArray[teamNr] = spaTeamCountsArray[teamNr]+1;
}

server.route('/api')
	.get(function(req, res, next) {
		res.json(
			{
				team: getSpaCounterRaceTeamName(req.session),
				count: getSpaCounterTeamCount(req.session)
			});
	});

server.route('/api/up')
	.post(function(req, res, next) {
		incSpaCounterRaceTeamCount(req.session);
		res.json(
			{
				team: getSpaCounterRaceTeamName(req.session),
				count: getSpaCounterTeamCount(req.session)
			});
	});


server.listen(process.env.PORT || 3000);
console.log("Server started at http://localhost:3000/");
