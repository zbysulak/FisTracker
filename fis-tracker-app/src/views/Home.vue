<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
        <h1 class="display-2 font-weight-bold mb-3">Welcome</h1>

        <p class="subheading font-weight-regular">Track your time easily.</p>
      </v-col>
    </v-row>
    <v-row v-if="isLogged">
      <v-col cols="12">
        <time-sheet></time-sheet>
      </v-col>
    </v-row>
    <v-row v-else> prihlas se! </v-row>
  </v-container>
</template>

<script>
import TimeSheet from "../components/TimeSheet.vue"

export default {
  name: "Home",

  components: {
    TimeSheet
  },

  data: () => ({
    isLogged: false
  }),
  watch: {
    "$store.state.user": {
      handler: function () {
        if (!this.$store.state.user.token) {
          this.isLogged = false
        } else {
          this.isLogged = true
        }
      },
      immediate: true // provides initial (not changed yet) state
    }
  }
}
</script>
