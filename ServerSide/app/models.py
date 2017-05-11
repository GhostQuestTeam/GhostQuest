from sqlalchemy import event

from database import db

class User(db.Model):
    __tablename__ = 'users'

    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(63), nullable=False, unique=True)
    email = db.Column(db.String(127), nullable=False, unique=True)
    score = db.Column(db.Integer)
    games = db.relationship('Game', backref='user')
    
    def __init__(self, id, name, email, score=0, games=[]):
		self.id = id
		self.name = name
		self.email = email
		self.score = score
		self.games = games

    def __str__(self):
        return self.name
        
        
class GameSession(db.Model):
	__tablename__ = 'gameSessions'
	
	id = db.Column(db.Integer, primary_key=True)
	user_id = db.Column(db.Integer, db.ForeignKey('user.id'))
	active = db.Column(db.Boolean)
	


@event.listens_for(Entity, 'after_delete')
def event_after_delete(mapper, connection, target):
    # Здесь будет очень важная бизнес логика
    # Или нет. На самом деле, старайтесь использовать сигналы только
    # тогда, когда других, более правильных вариантов не осталось.
    pass
