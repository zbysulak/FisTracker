<template>
    <div>
        <v-row class="justify-center">
             <v-switch
                class="mr-4"
                v-model="lunchSwitch"
                :label="`Lunch: ${lunchSwitch.toString()}`"
            ></v-switch>
            <v-switch
                v-model="businessTrip"
                :label="`Lunch: ${businessTrip.toString()}`"
            ></v-switch>
        </v-row>
        <v-row class="justify-center mb-8">
            <v-btn @click="start" class="ma-2 font-weight-bold attBtn" color="success" height="120" width="120">IN</v-btn>
            <v-btn @click="stop" class="ma-2 font-weight-bold attBtn" color="error" height="120" width="120">OUT</v-btn>
        </v-row>
    </div>
</template>

<script>

export default {
  name: "attendanceButtons",
  data: () => ({
    lunchSwitch: false,
    businessTrip: false
  }),

  methods: {
      start() {
          this.$root.snack.show({ message: "Successfully check in!" })
      },

      stop() {
          this.$root.snack.show({ message: "Successfully check out!" })
      }
  }
}
</script>

<style scoped>
    .attBtn {
        font-size: 25px;
    }
</style>