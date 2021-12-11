<template>
  <v-skeleton-loader
    v-if="loading"
    class="mx-auto"
    max-width="300"
    type="card"></v-skeleton-loader>
  <v-card v-else>
    <v-card-title>Month Overview</v-card-title>
    <v-card-text>
      <p class="mb-1">
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <span v-bind="attrs" v-on="on"
              >Required hours: <b>{{ totalTimeInMonth }}</b></span
            >
          </template>
          <span>Required number of hours in current month</span>
        </v-tooltip>
      </p>
      <p class="mb-1">
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <span v-bind="attrs" v-on="on"
              >Worked time: <b>{{ totalTime }}</b></span
            >
          </template>
          <span>The time you spent at work</span>
        </v-tooltip>
      </p>
      <p class="mb-1">
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <span v-bind="attrs" v-on="on"
              >Remaining time: <b>{{ remainingTimeNeeded }}</b></span
            >
          </template>
          <span>Remaining time to work this month</span>
        </v-tooltip>
      </p>
      <v-tooltip bottom>
        <template v-slot:activator="{ on, attrs }">
          <span v-bind="attrs" v-on="on"
            >Average time: <b>{{ averageTimeNeeded }}</b></span
          >
        </template>
        <span>Average time you have to work for rest of this month</span>
      </v-tooltip>
    </v-card-text>
  </v-card>
</template>

<script>
function formatTimeSpan(ts) {
  return ts.days * 24 + ts.hours + ":" + ts.minutes.toString().padStart(2, "0")
}
export default {
  name: "OverviewPanel",
  props: {
    time: { type: Object, require: true },
    loading: { type: Boolean, default: false }
  },

  computed: {
    // number of working days in month * 8
    totalTimeInMonth() {
      console.log(this.time)
      if (this.time != undefined) {
        return formatTimeSpan(this.time.totalTimeNeeded)
      } else {
        return "jeste nic"
      }
    },
    // time worked this month
    totalTime() {
      if (this.time != undefined) {
        return formatTimeSpan(this.time.totalTimeWorked)
      } else {
        return "jeste nic"
      }
    },

    // remaining hours needed
    remainingTimeNeeded() {
      if (this.time != undefined) {
        return formatTimeSpan(this.time.remainingTimeNeeded)
      } else {
        return "jeste nic"
      }
    },

    averageTimeNeeded() {
      if (this.time != undefined) {
        return formatTimeSpan(this.time.averageTimeNeeded)
      } else {
        return "jeste nic"
      }
    }
  }
}
</script>
