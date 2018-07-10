from coevolutionary import fight
import pandas as pd


def sum_element(r):
	if r != r: #Yeah, this is used to check if a number is NaN :P
		return 0
	total = 0
	for e in r:
		total+=e
	return total

decks_to_use = ["A","B","C"]
file_name = "test.individuals"
with open(file_name, "r") as fp: lines = fp.readlines()
num_games = filter(None, lines[0])
individuals = {}
for line in lines:
	if line.startswith("DECK:"):
		decks_to_use.append(line)
	else:
		str_individual = line[line.find("[") + 1:line.find(")]")]
		name_individual = line.split(",")[-1]
		individuals.update({name_individual:str_individual})


df = pd.DataFrame(index=individuals.keys(), columns=individuals.keys())

str_battles = ""
for i, d1 in enumerate(decks_to_use):
	for j,d2 in enumerate(decks_to_use):
		str_battles = str_battles+" "+d1+"-"+d2+","

print(str_battles)



for ind1 in individuals.keys():
	for ind2 in individuals.keys():
		battles1=[]
		battles2=[]
		if ind1 != ind2:
			for i,d1 in enumerate(decks_to_use):
				for j,d2 in enumerate(decks_to_use):
					w1,w2 = fight(individuals[ind1],individuals[ind2],d1,d2)
					battles1.append(w1)
					battles2.append(w2)
			df[ind1][ind2] = battles1
			df[ind2][ind1] = battles2

#with pd.option_context('display.max_rows', 4, 'display.max_columns', 4):

print(df)

print("Total of victories per individual:")
df=df.applymap(sum_element)
print(df)

print("TOTAL GAMES "+str(df.values.sum()))
