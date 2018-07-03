from random import Random
from time import time
import inspyred


def generate_weights(random, args):
    size = args.get('num_weights')
    return [random.uniform(0, 1) for i in range(size)]

def evaluate_hearthstone(candidates, args):
	fitness = []
	for c in candidates:
		print('Candidate {0} has a length of {1}'.format(c,len(c)))
		total = 0
		for i in range(0,len(c)):
			total += c[i]
			print "Total is " +str(total)
		fitness.append(total)
	return fitness


def main(prng=None, display=False):
	if prng is None:
		prng = Random()
		prng.seed(time())


	ea = inspyred.ec.ES(prng)
	ea.terminator = [inspyred.ec.terminators.generation_termination] #inspyred.ec.terminators.diversity_termination

	ea.observer = [inspyred.ec.observers.stats_observer, inspyred.ec.observers.file_observer]
	final_pop = ea.evolve(generator=generate_weights,
						  num_weights = 10,
						  evaluator=evaluate_hearthstone,
						  pop_size=10,
						  bounder=inspyred.ec.Bounder(0,1),
						  maximize=True,
						  max_generations=100)

	if display:
		best = max(final_pop)
		print('Best Solution: \n{0}'.format(str(best)))
	return ea


if __name__ == '__main__':
	main(display=True)
