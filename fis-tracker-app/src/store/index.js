import Vue from "vue";
import Vuex from "vuex";
import axios from "axios";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    count: 0,
    user: {},
  },
  mutations: {

    TEST: (state) => state.count++,

    /*changeRunningState(state, status) {
      state.isRunning = status
    },*/

    GET_NAME(state, name) {
      state.user = name
    }
  },
  actions: {
    getUser({ commit }) {
      axios.get('https://dev.api.project.devg.cz/api/user').then(response => {
        commit('GET_NAME', response.data)
      })

    },
  },
  modules: {}
});
