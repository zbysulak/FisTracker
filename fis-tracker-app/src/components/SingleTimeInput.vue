<template>
  <v-card>
    <v-row>
      <v-col>
        {{ item.date }}
      </v-col>
      <v-col>
        <time-picker-input v-bind:time="item.in"></time-picker-input>
      </v-col>
      <v-col>
        <time-picker-input v-bind:time="item.out"></time-picker-input>
      </v-col>
      <v-col
        ><v-btn icon @click="edit(item.id)"
          ><v-icon>mdi-pencil</v-icon></v-btn
        ></v-col
      >
    </v-row>
  </v-card>
</template>
<script>
import TimePickerInput from "./TimePickerInput.vue"
export default {
  components: { TimePickerInput },
  props: ["item"],
  methods: {
    edit: function (id) {
      console.log("request to edit input #" + id)
    }
  }
}
</script>
