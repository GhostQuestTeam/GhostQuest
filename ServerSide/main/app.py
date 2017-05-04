from flask import Flask

app = Flask(__name__)

@app.route('/', methods=['GET'])
@app.route('/index', methods=['GET'])
def index():
    return "Вы сделали удачный пинг на сервер игры 'Haunted City'!"
    
@app.route('/register', methods=['PUT'])
def signup_redirect():
	return "Здесь должен быть редирект на регистрацию!"
	
@app.route('/authorize', methods=['PUT'])
def authorize_redirect():
	return "Здесь должен быть редирект на авторизацию!"
	
@app.route('/session', methods=['GET', 'DELETE'])
def session_redirect():
	return "Здесь должен быть редирект на чтение или удаление авторизационных данных!"
	
@app.route('/profile/me', methods=['GET', 'POST'])
def profile_me():
	return "Здесь должен выдаваться JSON с профилем запросившего игрока и проверкой авторизации!"
	
@app.route('/profile/<userId>', methods=['GET'])
def profile_byId(userId):
	return "Здесь должен выдаваться JSON с профилем игрока и проверкой авторизации запросившего!"
	
@app.route('/player', methods=['GET', 'POST'])
def player():
	return "Здесь должен выводиться JSON с информацией о боевых качествах и успехах игрока!"
	
@app.route('/game/part', methods=['PUT'])
def join_game():
	return "Это метод для подачи заявки на вступление в игру!"
	
@app.route('/game/<gameSessionId>', methods=['DELETE'])
def leave_game(gameSessionId):
	return "Это метод для выхода из одной из своих игр!"
	
@app.route('/zones', methods=['GET'])
def list_of_zones():
	return "Здесь должен быть JSON со списком ближних точек!"
	
@app.route('/zones/<zoneId>', methods=['GET'])
def zone_info(zoneId):
	return "Здесь должен быть JSON с подробной информацией по точке с выбранным идентификатором!"
	
@app.route('/game/shoot', methods=['PUT'])
def make_shot():
	return "Выстрел произведён!"


if __name__ == '__main__':
    app.run(debug=True)
