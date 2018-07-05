from random import Random
from random import randint
from time import time
import inspyred
#from inspyred.ec import Individual

DEBUG = True
DECKS = ["MAGE", "SHAMAN" , "WARRIOR"]
NUM_GAMES = 10

def generate_weights(random, args):
	size = args.get('num_weights')
	return [random.uniform(0, 1) for i in range(size)]


def individual_to_commandline(ind):
	#if ind is Individual get only the num_weights! else check that ind is a list of num_weights
	param = ""
	for e in ind:
		param = param+str(e)+"#"
	param = param[:-1]
	return param

def fight(f1, f2, d1, d2):
	w = randint(0,10)
	w1 = w
	w2 = 10 - w
	cml1 = individual_to_commandline(f1)
	cml2 = individual_to_commandline(f2)

	if DEBUG: print "\t\tdotnet run {0} {1} {2} {3} {4}".format(cml1,d1,cml2,d2,NUM_GAMES)
	return w1, w2

def evaluate_hearthstone(candidates, args):
	parents = args['_ec'].population

	to_fight = []
	for p in parents:
		n = len(p.candidate) // 2 #removing the half (taus)
		to_fight.append(p.candidate[:n])
	num_parents = len(parents)

	to_fight = to_fight + candidates
	victories = []

	fitness = []

	for i in range(0,len(to_fight)):
		victories.append({})
		victories[i]["TOTAL"] = 0
		for d1 in DECKS:
			for d2 in DECKS:
				victories[i][d1+d2] = 0

	for i,f1 in enumerate(to_fight):
		if DEBUG: print "INDIVIDUAL "+str(f1)
		for j,f2 in enumerate(to_fight):
			for d1 in DECKS:
				for d2 in DECKS:
					if i<=j:
						if DEBUG: print "\tVERSUS " + str(f2)
						#if DEBUG: print ("\t\tCONFRONTING {0} vs {1} Ind{2} vs Ind{3}".format(d1,d2,f1,f2))
						v1,v2 = fight(f1,f2,d1,d2)
						victories[i]["TOTAL"]+= v1
						victories[i][d1+d2] += v1
						victories[j]["TOTAL"]+=v2
						victories[j][d2+d1] += v2
						#if DEBUG: print "\t\t"+str(v1)+" "+str(v2)
	for i,v in enumerate(victories):
		if i<num_parents:
			print("Updating parent fitness")
		else:
			fitness.append(victories[i]["TOTAL"])


	return fitness


def main(prng=None, display=False):
	if prng is None:
		prng = Random()
		prng.seed(time())

	ea = inspyred.ec.ES(prng)
	ea.terminator = [inspyred.ec.terminators.evaluation_termination] #inspyred.ec.terminators.diversity_termination

	ea.observer = [inspyred.ec.observers.stats_observer, inspyred.ec.observers.file_observer]
	final_pop = ea.evolve(generator=generate_weights,
						  num_weights = 2,
						  evaluator=evaluate_hearthstone,
						  pop_size=5,
						  bounder=inspyred.ec.Bounder(0,1),
						  maximize=True,
						  max_evaluations=1000)

	if display:
		best = max(final_pop)
		print('Best Solution: \n{0}'.format(str(best)))
	return ea


if __name__ == '__main__':
	main(display=True)
