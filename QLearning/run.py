# This is for unreliable prediction
# Importing the libraries
from matplotlib.pyplot import table
import numpy as np
import pandas as pd
import random
import time
import socket
import os
# get all possible action-obj pairs
# UDP
localIP = "127.0.0.1"
localPort = 5500
bufferSize = 1024
message = ""
address = ""
action_done = 0

# Create a datagram socket
UDPServerSocket = socket.socket(
    family=socket.AF_INET, type=socket.SOCK_DGRAM)


def getList(dict):
    return list(dict.keys())


def get_combinations(df):
    # all sub actions and objs involved
    sub_action_list = []
    obj_list = []
    # each action
    for row in range(len(df)):
        row_list = df.loc[row, :].values.tolist()
        action_obj = row_list[1:]
        action_obj = [x for x in action_obj if type(x) == str]

        sub_action_list.extend([x.split("|")[0].strip() for x in action_obj])
        obj_list.extend([x.split("|")[1].strip() for x in action_obj])

    sub_action_list = list(set(sub_action_list))
    obj_list = list(set(obj_list))
    # print("All sub-actions: ", sub_action_list)
    # print("All objects: ", obj_list)
    # all possible action-obj pairs
    all_combinations = [i+" | "+j for i in sub_action_list for j in obj_list]
    random.shuffle(all_combinations)
    indices = list(range(len(all_combinations)))
    action_to_idx = dict(zip(all_combinations, indices))
    # print("\nAll sub-action-object pairs: ", action_to_idx)
    return action_to_idx


def extract_actions(action_to_idx, df, action):
    # This extracts the action sequence
    in_between_actions = 2
    action_seq = df.loc[df['action'] == action].values[0][1:]
    # Remove Nans
    action_seq = action_seq[~pd.isnull(action_seq)]
    # print(action_seq)
    all_action_list = getList(action_to_idx)
    n = len(action_seq)
    total_number_of_actions = (n) * in_between_actions
    i = 0
    append_counter = 0
    Q_table_dimensions = []
    main_action_index = 0
    while(i < total_number_of_actions):
        if(i % in_between_actions == 0):
            Q_table_dimensions.append(action_seq[main_action_index])
            main_action_index += 1
        else:
            if(all_action_list[append_counter] in Q_table_dimensions):
                append_counter += 1
            else:
                Q_table_dimensions.append(all_action_list[append_counter])
            append_counter += 1
            # print(main_action_index)
            # print(i)
        i += 1
    return (Q_table_dimensions)


def generate_reward_table(action_sequence):
    n = len(action_sequence)
    Reward_Table = np.array(np.zeros([n, n]))
    i = 0
    # Generate Rewards
    while(i < (n-1)):
        Reward_Table[i, i+1] = 1
        i += 1
    i = 0
    while(i < (n-3)):
        Reward_Table[i, i+3] = 1
        i += 1
    i = 0
    while(i < (n)):
        Reward_Table[i, i] = 1
        i += 1
    # print(Reward_Table.astype(int))
    return Reward_Table


# Making a function that returns the shortest route from a starting to ending location
def route_passive(starting_location, ending_location, R, n, location_to_state, state_to_location):
    R_new = np.copy(R)
    ending_state = location_to_state[ending_location]
    R_new[ending_state, ending_state] = 1000
    Q = np.array(np.zeros([n, n]))
    for i in range(1000):
        # print(Q.astype(int))
        # print("\n")
        current_state = np.random.randint(0, n)
        playable_actions = []
        for j in range(n):
            if R_new[current_state, j] > 0:
                playable_actions.append(j)
        next_state = np.random.choice(playable_actions)
        TD = R_new[current_state, next_state] + gamma * Q[next_state,
                                                          np.argmax(Q[next_state, ])] - Q[current_state, next_state]
        Q[current_state, next_state] = Q[current_state, next_state] + alpha * TD
    route = [starting_location]
    next_location = starting_location
    while (next_location != ending_location):
        starting_state = location_to_state[starting_location]
        next_state = np.argmax(Q[starting_state, ])
        next_location = state_to_location[next_state]
        route.append(next_location)
        starting_location = next_location
    return route

# get all possible action-obj pairs


def get_all_combinations(df):
    # all sub actions and objs involved
    sub_action_list = []
    obj_list = []
    # each action
    for row in range(len(df)):
        row_list = df.loc[row, :].values.tolist()
        action_obj = row_list[1:]
        action_obj = [x for x in action_obj if type(x) == str]

        sub_action_list.extend([x.split("|")[0].strip() for x in action_obj])
        obj_list.extend([x.split("|")[1].strip() for x in action_obj])

    sub_action_list = list(set(sub_action_list))
    obj_list = list(set(obj_list))
    # print("all sub-actions: ", sub_action_list)
    # print("all objects: ", obj_list)
    # all possible action-obj pairs
    all_combinations = [i+" | "+j for i in sub_action_list for j in obj_list]
    all_combinations.sort()
    indices = list(range(len(all_combinations)))
    action_to_idx = dict(zip(all_combinations, indices))
    # print("\nall sub-action-object pairs: ", action_to_idx)
    return action_to_idx


def make_reward_table(action_to_idx, idx_to_action, df, action, curr_state):
    n_all = len(action_to_idx.keys())
    reward_table = np.zeros([n_all, n_all])
    # the full row of sub-actions
    action_seq = [item for item in list(
        df.loc[df['action'] == action].values[0][1:]) if type(item) != float]
    n = len(action_seq)
    """
    # ----- binomial -----
    # locate current sub action
    current_sub = action_seq.index(curr_state)
    if current_sub < n:
        current_sub += 1
    print("current subaction index: ", current_sub)
    # for binomial distribution, number of trials
    r_values = list(range(n + 1))
    # binomial distribution of reward
    if current_sub < n:
        prob = current_sub/n
    else:
        prob = 0.99
    # the weights assigned to each sub-action from a binomial distribution
    dist_shrink = [binomial_param*binom.pmf(r, n, prob) for r in r_values ]
    unshrinked_dist = [binom.pmf(r, n, prob) for r in r_values ]
    # multiple same-value rewards
    dist_shrink[current_sub+1] = max(dist_shrink)
    unshrinked_dist[current_sub+1] = max(unshrinked_dist)
    print("weights for each sub-action: ", dist_shrink)
    # ----- end of binomial -----
    """
    # --- end of linear reward ---
    for i in range(n-1):
        input = action_seq[i]
        output = action_seq[i+1]
        # print("output: ", output, type(output))
        in_coord = action_to_idx[input]
        out_coord = action_to_idx[output]
        # linear distribution of reward
        reward = round((i+1) * (1/n), 6)
        # print("reward", reward)
        # binomial distribution of reward
        # reward = dist_shrink[i]
        # print("reward: ", reward)
        reward_table[in_coord][out_coord] = reward
    """
    # --- binomial ---
    # Debug
    print("---------------------")
    print("Action Seq : ",action_seq)
    print("Prob :",unshrinked_dist)
    print("---------------------")
    reward_table[out_coord][out_coord] = dist_shrink[-1]
    # assign each all-zero rows with a weight thta leads it to the starting action
    starting_coord = action_to_idx[action_seq[0]]
    smallest = min(dist_shrink)
    for j in range(len(reward_table)):
        if reward_table[j][starting_coord] == 0:
            reward_table[j][starting_coord] = smallest
    # --- end of binomial ---
    """
    # --- linear reward ---
    starting_coord = action_to_idx[action_seq[0]]
    for j in range(len(reward_table)):
        if reward_table[j][starting_coord] == 0:
            reward_table[j][starting_coord] = 0.1
    reward_table[out_coord][out_coord] = 1
    return reward_table

# the function for Q-learning


def find_route(df, action, starting_location, ending_location, action_to_idx, idx_to_action, gamma, alpha, episodes, timeout):
    # R_new is the reward table, current sub-action is the 0-th sub action
    R_new = make_reward_table(
        action_to_idx, idx_to_action, df, action, starting_location)
    pd.DataFrame(R_new).to_csv("R_new.csv", header=False, index=False)
    n_all = len(R_new[0])
    Q = np.zeros([n_all, n_all])
    t0 = time.perf_counter()
    for i in range(episodes):
        # if i%1000 == 0:
        # print("i", i)
        current_state = np.random.randint(0, n_all)
        playable_actions = []
        for j in range(n_all):
            if R_new[current_state, j] > 0:
                playable_actions.append(j)
        next_state = np.random.choice(playable_actions)
        # print("next state:", idx_to_action[next_state])
        TD = R_new[current_state, next_state] + gamma * Q[next_state,
                                                          np.argmax(Q[next_state, ])] - Q[current_state, next_state]
        Q[current_state, next_state] = Q[current_state, next_state] + alpha * TD
        # pd.DataFrame(R_new).to_csv("R_new.csv", header = False, index = None)
        # print("next_state: ", next_state)
    route = [starting_location]
    next_location = starting_location
    # while (next_location != ending_location):
    starting_state = action_to_idx[starting_location]
    next_state = np.argmax(Q[starting_state, ])
    next_location = idx_to_action[next_state]
    route.append(next_location)
    starting_location = next_location
    t1 = time.perf_counter()
    time_taken = t1 - t0
    # if the maximum is never found
    # if time_taken >= timeout:
    # return "timeout"
    # print("starting and next", starting_location)
    Q_df = pd.DataFrame(Q)
    Q_df.to_csv("Q.csv", header=None, index=False)
    # print("\ntrain time: ", time_taken)
    return route


def predict_single(alpha, gamma, starting_location, ending_location, action):
    ########################## create action|object pairs ###############################
    df = pd.read_csv("action.csv")
    action_to_idx = get_all_combinations(df)
    idx_to_action = {idx: action for action, idx in action_to_idx.items()}

    ############################### training #####################################
    # Setting the parameters gamma and alpha for the Q-Learning
    episodes = 4000
    timeout = 30

    full_route = [starting_location]
    while starting_location != ending_location:
        route = find_route(df, action, starting_location, ending_location,
                           action_to_idx, idx_to_action, gamma, alpha, episodes, timeout)
        # print(route)
        msgFromServer = ''.join(
            i for i in starting_location if not i.isdigit())
        bytesToSend = str.encode(msgFromServer)
        UDPServerSocket.sendto(bytesToSend, address)
        print(''.join(i for i in starting_location if not i.isdigit()))
        bytesAddressPair = UDPServerSocket.recvfrom(bufferSize)
        starting_location = route[-1]
        full_route.append(starting_location)


def predict_multiple(alpha, gamma, action):
    df = pd.read_csv("action.csv")
    combinations = get_combinations(df)
    action_sequence = extract_actions(combinations, df, action)
    R_table = generate_reward_table(action_sequence)
    actions = list(range(len(action_sequence)))
    location_to_state = dict(zip(action_sequence, actions))
    # print(actions)
    # print(location_to_state)
    # Making a mapping from the states to the locations
    state_to_location = {state: location for location,
                         state in location_to_state.items()}
    # print(state_to_location)
    try:
        output = route_passive(action_sequence[0], action_sequence[len(
            action_sequence)-2], R_table, len(action_sequence), location_to_state, state_to_location)
    except:
        output = ["timeout"]
    # print(output)
    for action in output:
        msgFromServer = ''.join(i for i in action if not i.isdigit())
        bytesToSend = str.encode(msgFromServer)
        UDPServerSocket.sendto(bytesToSend, address)
        print(''.join(i for i in action if not i.isdigit()))
        bytesAddressPair = UDPServerSocket.recvfrom(bufferSize)
        time.sleep(0.4)


def predict_action(alpha, gamma, start_action, end_action, action):
    ran = random.randint(1, 100)
    if(ran > random.randint(80, 90)):
        predict_multiple(alpha, gamma, action)
    else:
        predict_single(alpha, gamma, start_action, end_action, action)


# def UDPcommunicate(UDPServerSocket, bufferSize, messagetosend):
#     bytesAddressPair = UDPServerSocket.recvfrom(bufferSize)
#     message = bytesAddressPair[0]
#     address = bytesAddressPair[1]
#     msgFromServer = messagetosend
#     bytesToSend = str.encode(msgFromServer)
#     UDPServerSocket.sendto(bytesToSend, address)
#     return(message)


if __name__ == "__main__":
    # Bind to address and ip
    UDPServerSocket.bind((localIP, localPort))
    os.system("clear")
    print("   ______     ___  _   _ __     __")
    print("  / ___\ \   / / || | | |\ \   / /")
    print(" | |    \ \ / /| || |_| | \ \ / / ")
    print(" | |___  \ V / |__   _| |__\ V /  ")
    print("  \____|  \_/     |_| |_____\_/   ")
    print("\nLifan Yu (ly1164@nyu.edu) - MMVC Lab\nNew York University Abu Dhabi\n")
    print("CV4LV connection set up complete. \nPlease start the simulation now.\n")

    ########################## create action|object pairs ###############################
    action_list = ["null", "microwave food",
                   "cut vegetable",
                   "make cereal",
                   "make salad",
                   "use oven",
                   "cook soup",
                   "peel"]

    # action_to_process = int(UDPcommunicate(
    #     UDPServerSocket, bufferSize, "Predicting Actions").decode("utf-8"))

    # Get UDP
    while(True):
        bytesAddressPair = UDPServerSocket.recvfrom(bufferSize)
        message = bytesAddressPair[0]
        address = bytesAddressPair[1]
        action_int = int(message.decode("utf-8"))
        if(action_int < 9):
            action = action_list[action_int]
            print("\n-----------------------------\nNew Action - " +
                  action_list[action_int]+" - selected.\n")
            # Setting the parameters gamma and alpha for the Q-Learning
            gamma = 0.75
            alpha = 0.9
            df = pd.read_csv("action.csv")
            combinations = get_combinations(df)
            action_sequence = extract_actions(combinations, df, action)
            predict_action(alpha, gamma, action_sequence[0], action_sequence[len(
                action_sequence)-2], action)
        print("Action Done")
        msgFromServer = "Action Done"
        bytesToSend = str.encode(msgFromServer)
        UDPServerSocket.sendto(bytesToSend, address)
