from coevolutionary import fight
from coevolutionary import NUM_GAMES as NG
from coevolutionary import NUM_WEIGHTS as NW
import pandas as pd

NUM_GAMES = NG #WARNING, THE NUMBER OF GAMES AND WEIGHTS IS DEFINED IN THE OTHER FILE!
NUM_WEIGHTS = NW




def sum_element(r):
	if r != r: #Yeah, this is used to check if a number is NaN :P
		return 0
	total = 0
	for e in r:
		total+=e
	return total

decks_to_use=[]
file_name = "firsttest.individuals"
with open(file_name, "r") as fp: lines = fp.readlines()
num_games = filter(None, lines[0])
individuals = {}
for line in lines:
	if line.startswith("DECK:"):
		decks_to_use.append(line[line.find("DECK:")+5:-1])
	else:
		str_individual = line[line.find("[") + 1:line.find("]")]
		individual_list = [float(i) for i in str_individual.split(",")]
		individual_list = individual_list[:NUM_WEIGHTS]
		name_individual = line.split(",")[-1]
		individuals.update({name_individual:individual_list})


num_games_per_vs=NUM_GAMES*len(decks_to_use)**2
num_games_per_vs_all=num_games_per_vs*(len(individuals)-1) #NOT USING SELF VS!!!
print("Number of games of Individual vs Individual: "+str(num_games_per_vs))
print("Number of games of Individual vs all Individuals: "+str(num_games_per_vs_all) )


df = pd.DataFrame(index=individuals.keys(), columns=individuals.keys())

str_battles = ""
for i, d1 in enumerate(decks_to_use):
	for j,d2 in enumerate(decks_to_use):
		str_battles = str_battles+" "+d1+"-"+d2+","

print(str_battles)



for i,ind1 in enumerate(individuals.keys()):
	for j,ind2 in enumerate(individuals.keys()):
		battles1=[]
		battles2=[]
		if i<j:
			print(str(ind1)+"vs"+str(ind2))
			for d1 in decks_to_use:
				for d2 in decks_to_use:
					w1,w2 = fight(individuals[ind1],individuals[ind2],d1,d2)
					battles1.append(w1)
					battles2.append(w2)
			print(str(ind1)+" WON "+str(sum(battles1)))
			print(str(ind2)+" WON "+str(sum(battles2)))
			#df[ind1][ind2] = battles1
			#df[ind2][ind1] = battles2
			df.set_value(ind1,ind2,battles1)
			df.set_value(ind2,ind1,battles2)

with pd.option_context('display.max_colwidth', 100,'display.max_columns',100,'display.max_rows',100):
	print(df)

print("Total of victories per combination:")
df_sum=df.applymap(sum_element)
print(df_sum)
print(df_sum/num_games_per_vs)

print("Total victories grouped by individual")
print(df_sum.sum(axis=1))
print(df_sum.sum(axis=1)/num_games_per_vs_all)

print("TOTAL GAMES "+str(df_sum.values.sum()))

